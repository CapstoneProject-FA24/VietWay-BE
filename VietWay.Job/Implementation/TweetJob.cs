using Microsoft.EntityFrameworkCore;
using Tweetinvi.Core.Extensions;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Service.ThirdParty.Twitter;

namespace VietWay.Job.Implementation
{
    public class TweetJob(IUnitOfWork unitOfWork, ITwitterService twitterService, IRedisCacheService redisCacheService) : ITweetJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITwitterService _twitterService = twitterService;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        public async Task GetPublishedTweetsJob()
        {
            var socialMediaPosts = await _unitOfWork.SocialMediaPostRepository.Query()
                .Where(x => x.Site == SocialMediaSite.Twitter)
                .ToListAsync();

            if (socialMediaPosts.IsNullOrEmpty())
            {
                return;
            }

            List<TweetDTO> tweetsDetails = await _twitterService.GetTweetsAsync(socialMediaPosts.Select(x => x.SocialPostId).ToList());

            var tweetLookup = tweetsDetails.ToLookup(x => x.XTweetId);

            var keyValuePairs = socialMediaPosts
                .Where(post => tweetLookup.Contains(post.SocialPostId))
                .GroupBy(post => post.EntityType switch
                {
                    SocialMediaPostEntity.Attraction => $"{post.AttractionId}-{(int)post.EntityType}",
                    SocialMediaPostEntity.Post => $"{post.PostId}-{(int)post.EntityType}",
                    SocialMediaPostEntity.TourTemplate => $"{post.TourTemplateId}-{(int)post.EntityType}",
                }).ToDictionary(
                    group => group.Key!,
                    group => group.SelectMany(post => tweetLookup[post.SocialPostId]).ToList()
                );

            await _redisCacheService.SetMultipleAsync(keyValuePairs);
        }

        public async Task GetPopularHashtagJob()
        {
            var hashtags = await _unitOfWork.HashtagRepository.Query().ToListAsync();

            if (hashtags.IsNullOrEmpty())
            {
                return;
            }

            Dictionary<string, int> hashtagCount = await _twitterService.GetHashtagCountsAsync(hashtags.Select(x => x.HashtagName).ToList());

            var systemHashtagNames = hashtags.Select(h => h.HashtagName).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var topHashtags = hashtagCount
                .Where(kvp => systemHashtagNames.Contains(kvp.Key))
                .OrderByDescending(kvp => kvp.Value)
                .Take(5)
                .Select(kvp => kvp.Key)
                .ToList();

            var topHashtagIds = hashtags
                .Where(h => topHashtags.Contains(h.HashtagName))
                .Select(h => h.HashtagId)
                .ToList();

            await _redisCacheService.SetAsync("popularHashtag", topHashtagIds);
        }
    }
}
