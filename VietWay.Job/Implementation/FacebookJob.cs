using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Facebook;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Job.Implementation
{
    public class FacebookJob(IUnitOfWork unitOfWork, ITimeZoneHelper timeZoneHelper, IFacebookService facebookService, IIdGenerator idGenerator, IRedisCacheService redisCacheService)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IFacebookService _facebookService = facebookService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        public async Task GetPublishedFacebookPostsJob()
        {
            DateTime createdDate = _timeZoneHelper.GetUTC7Now().AddDays(-90);
            List<string> postToGet = await _unitOfWork.SocialMediaPostRepository.Query()
                .Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= createdDate)
                .Select(x => x.SocialPostId)
                .ToListAsync();
            foreach (var post in postToGet)
            {
                var facebookReactions = await _facebookService.GetPostReactionCountByTypeAsync(post);
                var facebookComments = await _facebookService.GetPostCommentCountAsync(post);
                var facebookShares = await _facebookService.GetPostShareCountAsync(post);
                var impression = await _facebookService.GetPostImpressionCountAsync(post);
                FacebookPostMetric facebookPostMetric = new()
                {
                    CreatedAt = _timeZoneHelper.GetUTC7Now(),
                    AngerCount = facebookReactions.AngryCount,
                    CommentCount = facebookComments,
                    HahaCount = facebookReactions.HahaCount,
                    ImpressionCount = impression,
                    LikeCount = facebookReactions.LikeCount,
                    LoveCount = facebookReactions.LoveCount,
                    PostClickCount = 0,
                    ShareCount = facebookShares,
                    SorryCount = facebookReactions.SadCount,
                    WowCount = facebookReactions.WowCount,
                    MetricId = _idGenerator.GenerateId(),
                    SocialPostId = post,
                };
                await _unitOfWork.FacebookPostMetricRepository.CreateAsync(facebookPostMetric);
            }
            await _redisCacheService.SetAsync("facebookPostMetric", true, TimeSpan.FromHours(20));
        }
    }
}
