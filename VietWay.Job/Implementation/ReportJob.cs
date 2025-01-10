using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Job.Implementation
{
    public class ReportJob(IUnitOfWork unitOfWork, ITimeZoneHelper timeZoneHelper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task GenerateDailyAttractionReport()
        {
            DateTime startTime = _timeZoneHelper.GetUTC7Now().AddDays(-1).Date;
            DateTime endTime = startTime.AddHours(23).AddMinutes(59);
            var reports = await _unitOfWork.AttractionRepository.Query()
                .GroupBy(x => new { x.ProvinceId, x.AttractionCategoryId })
                .Select(g => new AttractionReport
                {
                    ProvinceId = g.Key.ProvinceId,
                    AttractionCategoryId = g.Key.AttractionCategoryId,
                    FacebookAngerCount = g.SelectMany(x=>x.SocialMediaPosts.Where(x=>x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x=>x.FacebookPostMetrics).Sum(x=>x.AngerCount),
                    FacebookCommentCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.CommentCount),
                    FacebookHahaCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.HahaCount),
                    FacebookImpressionCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.ImpressionCount),
                    FacebookLikeCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.LikeCount),
                    FacebookLoveCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.LoveCount),
                    FacebookShareCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.ShareCount),
                    FacebookWowCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.WowCount),
                    FacebookSorryCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.SorryCount),
                    XBookmarkCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.TwitterPostMetrics).Sum(x => x.BookmarkCount),
                    XImpressionCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.TwitterPostMetrics).Sum(x => x.ImpressionCount),
                    XLikeCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.TwitterPostMetrics).Sum(x => x.LikeCount),
                    XQuoteCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.TwitterPostMetrics).Sum(x => x.QuoteCount),
                    XReplyCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.TwitterPostMetrics).Sum(x => x.ReplyCount),
                    XRetweetCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.TwitterPostMetrics).Sum(x => x.RetweetCount),
                    FacebookClickCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook && x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .SelectMany(x => x.FacebookPostMetrics).Sum(x => x.PostClickCount),
                    FiveStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 5),
                    FourStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 4),
                    ThreeStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 3),
                    TwoStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 2),
                    OneStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 1),
                    FiveStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x=>x.Rating == 5)).SelectMany(x=>x.AttractionReviewLikes.Where(x=> x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    FourStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 4)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    ThreeStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 3)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    TwoStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 2)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    OneStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 1)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    SiteLikeCount = g.SelectMany(x => x.AttractionLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    FacebookReferralCount = g.SelectMany(x=>x.AttractionMetrics.Where(x=> x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x=>x.FacebookReferralCount)).Sum(),
                    XReferralCount = g.SelectMany(x => x.AttractionMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.XReferralCount)).Sum(),
                    SiteReferralCount = g.SelectMany(x => x.AttractionMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.SiteReferralCount)).Sum(),
                    
                }).ToListAsync();
        }
    }
}
