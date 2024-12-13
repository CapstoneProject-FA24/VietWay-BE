using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tweetinvi.Core.Extensions;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Facebook;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Service.ThirdParty.Twitter;
using VietWay.Util.CustomExceptions;

namespace VietWay.Service.Management.Implement
{
    public class PublishPostService(IUnitOfWork unitOfWork, ITwitterService twitterService, IFacebookService facebookService, 
        IRedisCacheService redisCacheService) : IPublishPostService
    {
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFacebookService _facebookService = facebookService;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;

        public async Task<FacebookMetricsDTO> GetFacebookPostMetricsAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("Post not found");
            if (post.FacebookPostId.IsNullOrEmpty())
            {
                throw new InvalidOperationException("The post has not been published");
            }
            Task<int> countCommentTask = _facebookService.GetPostCommentCountAsync(post.FacebookPostId!);
            Task<int> countShareTask = _facebookService.GetPostShareCountAsync(post.FacebookPostId!);
            Task<int> countImpressionTask = _facebookService.GetPostImpressionCountAsync(post.FacebookPostId!);
            Task<PostReaction> getReactionsTask = _facebookService.GetPostReactionCountByTypeAsync(post.FacebookPostId!);
            await Task.WhenAll(countCommentTask, countImpressionTask, countShareTask, getReactionsTask);
            return new FacebookMetricsDTO
            {
                CommentCount = countCommentTask.Result,
                ImpressionCount = countImpressionTask.Result,
                PostReactions = getReactionsTask.Result,
                ShareCount = countShareTask.Result
            };
        }

        public async Task<TweetDTO> GetPublishedTweetByIdAsync(string postId)
        {
            TweetDTO? tweetDto = await _redisCacheService.GetAsync<TweetDTO>(postId);
            if (null == tweetDto)
            {
                throw new ResourceNotFoundException("The post has not been published");
            }
            return tweetDto;
        }

        public async Task PostTweetWithXAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("Post not found");

            if (post.Status != PostStatus.Approved)
            {
                throw new ServerErrorException("Post has not been approved yet");
            }
            if (!post.XTweetId.IsNullOrEmpty())
            {
                throw new ServerErrorException("The post has already been tweeted");
            }

            PostTweetRequestDTO postTweetRequestDTO = new PostTweetRequestDTO
            {
                Text = $"{post.Title.ToUpper()}\n\nXem thêm tại: https://vietway.projectpioneer.id.vn/bai-viet/{post.PostId}",
                ImageUrl = post.ImageUrl
            };
            string result = await _twitterService.PostTweetAsync(postTweetRequestDTO);
            using JsonDocument document = JsonDocument.Parse(result);
            string tweetId = document.RootElement.GetProperty("data").GetProperty("id").GetString();
            
            try
            {
                if (tweetId.IsNullOrEmpty())
                {
                    throw new ServerErrorException("Post tweet error");
                }
                post.XTweetId = tweetId;
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.PostRepository.UpdateAsync(post);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task PublishPostToFacebookPageAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("Post not found");

            if (post.Status != PostStatus.Approved)
            {
                throw new ServerErrorException("Post has not been approved yet");
            }
            if (!post.FacebookPostId.IsNullOrEmpty())
            {
                throw new ServerErrorException("The post has already been tweeted");
            }

            string facebookPostId = await _facebookService.PublishPostAsync(post.Description, $"https://vietway.projectpioneer.id.vn/bai-viet/{post.PostId}");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                post.FacebookPostId = facebookPostId;
                await _unitOfWork.PostRepository.UpdateAsync(post);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteTweetWithXAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("Post not found");

            if (post.XTweetId.IsNullOrEmpty())
            {
                throw new ServerErrorException("The post has not been tweeted yet");
            }

            try
            {
                await _twitterService.DeleteTweetAsync(post.XTweetId);
                post.XTweetId = null;
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.PostRepository.UpdateAsync(post);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
