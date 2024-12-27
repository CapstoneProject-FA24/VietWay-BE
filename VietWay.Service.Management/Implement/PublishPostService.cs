using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models.V2;
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
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            List<SocialMediaPost> socialMediaPosts = await _unitOfWork.SocialMediaPostRepository.Query()
               .Where(x => x.EntityType == SocialMediaPostEntity.Post && x.EntityId == post.PostId && x.Site == SocialMediaSite.Facebook).ToListAsync();
            if (socialMediaPosts.Count <= 0)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_PUBLISHED");
            }

            Task<int> countCommentTask = _facebookService.GetPostCommentCountAsync(socialMediaPosts[0].SocialPostId!);
            Task<int> countShareTask = _facebookService.GetPostShareCountAsync(socialMediaPosts[0].SocialPostId!);
            Task<int> countImpressionTask = _facebookService.GetPostImpressionCountAsync(socialMediaPosts[0].SocialPostId!);
            Task<PostReaction> getReactionsTask = _facebookService.GetPostReactionCountByTypeAsync(socialMediaPosts[0].SocialPostId!);
            await Task.WhenAll(countCommentTask, countImpressionTask, countShareTask, getReactionsTask);
            return new FacebookMetricsDTO
            {
                CommentCount = countCommentTask.Result,
                ImpressionCount = countImpressionTask.Result,
                PostReactions = getReactionsTask.Result,
                ShareCount = countShareTask.Result
            };
        }

        public async Task<List<TweetDTO>> GetPublishedTweetByIdAsync(string postId)
        {
            List<TweetDTO> tweetDto = await _redisCacheService.GetAsync<List<TweetDTO>>(postId);
            if (null == tweetDto)
            {
                throw new ResourceNotFoundException("POST_NOT_PUBLISHED");
            }

            var socialMediaPosts = await _unitOfWork.SocialMediaPostRepository.Query()
                .Where(x => x.EntityId == postId && x.Site == SocialMediaSite.Twitter)
                .ToListAsync();

            foreach (var tweet in tweetDto)
            {
                var socialMediaPost = socialMediaPosts.FirstOrDefault(post => post.SocialPostId == tweet.XTweetId);
                if (socialMediaPost != null)
                {
                    tweet.CreatedAt = socialMediaPost.CreatedAt;
                }
            }

            return tweetDto;
        }

        public async Task PostTweetWithXAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            if (post.Status != PostStatus.Approved)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_APPROVED");
            }

            /*bool isPublished = await _unitOfWork.SocialMediaPostRepository.Query()
                .AnyAsync(x => x.EntityType == SocialMediaPostEntity.Post && x.EntityId == post.PostId && x.Site == SocialMediaSite.Twitter);
            if (isPublished)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_PUBLISHED");
            }*/

            PostTweetRequestDTO postTweetRequestDTO = new()
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
                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = tweetId,
                    Site = SocialMediaSite.Twitter,
                    EntityType = SocialMediaPostEntity.Post,
                    EntityId = post.PostId,
                    CreatedAt = DateTime.Now,
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);
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
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            if (post.Status != PostStatus.Approved)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_NOT_APPROVED");
            }

            /*bool isPublished = await _unitOfWork.SocialMediaPostRepository.Query()
                .AnyAsync(x => x.EntityType == SocialMediaPostEntity.Post && x.EntityId == post.PostId && x.Site == SocialMediaSite.Facebook);
            if (isPublished)
            {
                throw new InvalidActionException("INVALID_ACTION_POST_PUBLISHED");
            }*/

            string facebookPostId = await _facebookService.PublishPostAsync(post.Description, $"https://vietway.projectpioneer.id.vn/bai-viet/{post.PostId}");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                SocialMediaPost socialMediaPost = new()
                {
                    SocialPostId = facebookPostId,
                    Site = SocialMediaSite.Facebook,
                    EntityType = SocialMediaPostEntity.Post,
                    EntityId = post.PostId,
                    CreatedAt = DateTime.Now,
                };
                await _unitOfWork.SocialMediaPostRepository.CreateAsync(socialMediaPost);
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
