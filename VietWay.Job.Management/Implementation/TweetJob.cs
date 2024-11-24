using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Tweetinvi.Core.Extensions;
using VietWay.Job.Management.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Service.ThirdParty.Twitter;
using VietWay.Util.CustomExceptions;

namespace VietWay.Job.Management.Implementation
{
    public class TweetJob(IUnitOfWork unitOfWork, ITwitterService twitterService, IRedisCacheService redisCacheService) : ITweetJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        public async Task GetPublishedTweetsJob()
        {
            Dictionary<string, string> postTweetId = await _unitOfWork.PostRepository.Query()
                .Where(x => x.XTweetId != null)
                .ToDictionaryAsync(x => x.PostId!, x => x.XTweetId!);

            if (postTweetId.IsNullOrEmpty())
            {
                return;
            }

            List<TweetDTO> tweetsDetails = await _twitterService.GetTweetsAsync([.. postTweetId.Values]);
            Dictionary<string, TweetDTO> keyValuePairs = postTweetId
            .Where(pair => tweetsDetails.Any(tweet => tweet.XTweetId == pair.Value)) // Only valid matches
            .ToDictionary(
                pair => pair.Key,
                pair => tweetsDetails.First(tweet => tweet.XTweetId == pair.Value)
            );
            await _redisCacheService.SetMultipleAsync(keyValuePairs);
        }
    }
}
