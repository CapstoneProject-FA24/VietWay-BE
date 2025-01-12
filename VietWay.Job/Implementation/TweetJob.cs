using Microsoft.EntityFrameworkCore;
using Tweetinvi.Core.Extensions;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
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
            List<HashtagCountDTO> hashtagCounts = await _redisCacheService.GetAsync<List<HashtagCountDTO>>("hashtagCounts");
            var allHashtags = await _unitOfWork.HashtagRepository.Query().OrderBy(x => x.CreatedAt).ToListAsync();

            if (allHashtags.IsNullOrEmpty()) 
            {
                return; 
            }

            Hashtag currentHashtag;
            if (hashtagCounts.IsNullOrEmpty())
            {
                hashtagCounts = new List<HashtagCountDTO>();
                currentHashtag = allHashtags.First();
            }
            else
            {
                var currentHashtagId = hashtagCounts.FirstOrDefault(x => x.IsCurrent)?.HashtagId;

                if (currentHashtagId == null)
                {
                    currentHashtag = allHashtags.First();
                }
                else
                {
                    int currentIndex = allHashtags.FindIndex(x => x.HashtagId == currentHashtagId);
                    int nextIndex = (currentIndex + 1) % allHashtags.Count;
                    currentHashtag = allHashtags[nextIndex];

                    var previousHashtagCount = hashtagCounts.FirstOrDefault(x => x.HashtagId == currentHashtagId);
                    if (previousHashtagCount != null)
                    {
                        previousHashtagCount.IsCurrent = false;
                    }
                }
            }

            var currentHashtagCount = hashtagCounts.FirstOrDefault(x => x.HashtagId == currentHashtag.HashtagId);
            if (currentHashtagCount != null)
            {
                currentHashtagCount.Count = await _twitterService.GetHashtagCountsAsync(currentHashtag.HashtagName);
                currentHashtagCount.IsCurrent = true;
            }
            else
            {
                hashtagCounts.Add(new HashtagCountDTO
                {
                    HashtagId = currentHashtag.HashtagId,
                    Count = await _twitterService.GetHashtagCountsAsync(currentHashtag.HashtagName),
                    IsCurrent = true
                });
            }

            await _redisCacheService.SetAsync("hashtagCounts", hashtagCounts);
        }

    }
}
