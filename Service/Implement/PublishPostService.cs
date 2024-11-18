using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.Web;
using VietWay.Job.Interface;
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
    public class PublishPostService(IUnitOfWork unitOfWork, ITwitterService twitterService, IFacebookService facebookService, IRecurringJobManager recurringJobManager, ITweetJob tweetJob, IRedisCacheService redisCacheService) : IPublishPostService
    {
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFacebookService _facebookService = facebookService;
        private readonly IRecurringJobManager _recurringJobManager = recurringJobManager;
        private readonly ITweetJob _tweetJob = tweetJob;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;

        public async Task<int> GetPublishedPostReactionAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("Post not found");

            if (post.FacebookPostId.IsNullOrEmpty())
            {
                throw new ServerErrorException("The post has not been published");
            }
            return await _facebookService.GetPublishedPostReactionAsync(post.FacebookPostId!);
        }

        public async Task<List<TweetDTO>> GetPublishedTweetsAsync()
        {
            List<Post>? posts = await _unitOfWork.PostRepository.Query().Where(x => x.XTweetId != null).ToListAsync() ??
                throw new ResourceNotFoundException("No posts have been posted on X yet.");

            var tweetDTOs = new List<TweetDTO>();
            tweetDTOs = await _redisCacheService.GetAsync<List<TweetDTO>>("tweetsDetail") ?? new List<TweetDTO>();
            return tweetDTOs;
        }

        public async Task<TweetDTO> GetPublishedTweetByIdAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("Post not found");

            if (post.XTweetId.IsNullOrEmpty())
            {
                throw new ServerErrorException("The post has not been published");
            }
            var tweetDTOs = new List<TweetDTO>();
            tweetDTOs = await _redisCacheService.GetAsync<List<TweetDTO>>("tweetsDetail") ?? new List<TweetDTO>();

            return tweetDTOs.SingleOrDefault(x => x.PostId == postId);
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
                _recurringJobManager.AddOrUpdate("getTweetsDetail", () => _tweetJob.GetPublishedTweetsJob(), "*/16 * * * *");
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
