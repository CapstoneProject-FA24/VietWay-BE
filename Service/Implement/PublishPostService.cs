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
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Facebook;
using VietWay.Service.ThirdParty.Twitter;
using VietWay.Util.CustomExceptions;

namespace VietWay.Service.Management.Implement
{
    public class PublishPostService(IUnitOfWork unitOfWork, ITwitterService twitterService, IFacebookService facebookService) : IPublishPostService
    {
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IFacebookService _facebookService = facebookService;

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
                throw new ResourceNotFoundException("Posts not found");

            string tweetIds = string.Join(',', posts.Where(x => !x.XTweetId.IsNullOrEmpty()).Select(x => x.XTweetId));

            string jsonResponse = await _twitterService.GetTweetsAsync(tweetIds);

            var tweetData = JsonSerializer.Deserialize<JsonElement>(jsonResponse).GetProperty("data");

            var tweetDTOs = new List<TweetDTO>();

            foreach (var tweet in tweetData.EnumerateArray())
            {
                var tweetDTO = new TweetDTO
                {
                    XTweetId = tweet.GetProperty("id").GetString(),
                    RetweetCount = tweet.GetProperty("public_metrics").GetProperty("retweet_count").GetInt32(),
                    ReplyCount = tweet.GetProperty("public_metrics").GetProperty("reply_count").GetInt32(),
                    LikeCount = tweet.GetProperty("public_metrics").GetProperty("like_count").GetInt32(),
                    QuoteCount = tweet.GetProperty("public_metrics").GetProperty("quote_count").GetInt32(),
                    BookmarkCount = tweet.GetProperty("public_metrics").GetProperty("bookmark_count").GetInt32(),
                    ImpressionCount = tweet.GetProperty("public_metrics").GetProperty("impression_count").GetInt32()
                };

                var post = posts.SingleOrDefault(p => p.XTweetId == tweetDTO.XTweetId);
                if (post != null)
                {
                    tweetDTO.PostId = post.PostId;
                }

                tweetDTOs.Add(tweetDTO);
            }

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
            string jsonResponse = await _twitterService.GetTweetByIdAsync(post.XTweetId);
            var tweetData = JsonSerializer.Deserialize<JsonElement>(jsonResponse).GetProperty("data");

            var tweetDTO = new TweetDTO
            {
                PostId = post.PostId,
                XTweetId = tweetData.GetProperty("id").GetString(),
                RetweetCount = tweetData.GetProperty("public_metrics").GetProperty("retweet_count").GetInt32(),
                ReplyCount = tweetData.GetProperty("public_metrics").GetProperty("reply_count").GetInt32(),
                LikeCount = tweetData.GetProperty("public_metrics").GetProperty("like_count").GetInt32(),
                QuoteCount = tweetData.GetProperty("public_metrics").GetProperty("quote_count").GetInt32(),
                BookmarkCount = tweetData.GetProperty("public_metrics").GetProperty("bookmark_count").GetInt32(),
                ImpressionCount = tweetData.GetProperty("public_metrics").GetProperty("impression_count").GetInt32()
            };
            return tweetDTO;
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
    }
}
