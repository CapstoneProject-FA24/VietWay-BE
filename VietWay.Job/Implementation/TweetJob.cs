using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tweetinvi.Core.Extensions;
using VietWay.Job.DataTransferObject;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Service.ThirdParty.Twitter;
using VietWay.Util.CustomExceptions;

namespace VietWay.Job.Implementation
{
    public class TweetJob(IUnitOfWork unitOfWork, ITwitterService twitterService, IRedisCacheService redisCacheService) : ITweetJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;

        public async Task GetPublishedTweetsJob()
        {
            List<Post>? posts = await _unitOfWork.PostRepository.Query().Where(x => x.XTweetId != null).ToListAsync();
            if (posts.IsNullOrEmpty())
            {
                return;
            }

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

            await _redisCacheService.SetAsync<List<TweetDTO>>("tweetsDetail", tweetDTOs);
        }
    }
}
