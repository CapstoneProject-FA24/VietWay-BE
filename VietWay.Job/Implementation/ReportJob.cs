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
        public async Task GenerateDailyAttractionReport(DateTime dateTime)
        {
            DateTime startTime = dateTime.Date;
            DateTime endTime = startTime.AddHours(23).AddMinutes(59);
            var reports = await _unitOfWork.AttractionRepository.Query()
                .Where(x => x.AttractionMetrics.Any(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime) ||
                    x.SocialMediaPosts.Any(x => x.FacebookPostMetrics.Any(y => y.CreatedAt >= startTime && y.CreatedAt <= endTime) || x.TwitterPostMetrics.Any(y => y.CreatedAt >= startTime && y.CreatedAt <= endTime)))
                .GroupBy(x => new { x.ProvinceId, x.AttractionCategoryId })
                .Select(g => new AttractionReport
                {
                    ProvinceId = g.Key.ProvinceId,
                    AttractionCategoryId = g.Key.AttractionCategoryId,
                    FacebookAngerCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.AngerCount),
                    FacebookCommentCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.CommentCount),
                    FacebookHahaCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.HahaCount),
                    FacebookImpressionCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ImpressionCount),
                    FacebookLikeCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LikeCount),
                    FacebookLoveCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LoveCount),
                    FacebookShareCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ShareCount),
                    FacebookWowCount = g.SelectMany(x => x.SocialMediaPosts.Where(x =>x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.WowCount),
                    FacebookSorryCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.SorryCount),
                    XBookmarkCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.BookmarkCount),
                    XImpressionCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ImpressionCount),
                    XLikeCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LikeCount),
                    XQuoteCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.QuoteCount),
                    XReplyCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ReplyCount),
                    XRetweetCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x=>x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.RetweetCount),
                    FacebookClickCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.PostClickCount),
                    FiveStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 5),
                    FourStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 4),
                    ThreeStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 3),
                    TwoStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 2),
                    OneStarRatingCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(x => x.Rating == 1),
                    FiveStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 5)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    FourStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 4)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    ThreeStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 3)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    TwoStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 2)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    OneStarRatingLikeCount = g.SelectMany(x => x.AttractionReviews.Where(x => x.Rating == 1)).SelectMany(x => x.AttractionReviewLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    SiteLikeCount = g.SelectMany(x => x.AttractionLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    FacebookReferralCount = g.SelectMany(x => x.AttractionMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.FacebookReferralCount)).Sum(),
                    XReferralCount = g.SelectMany(x => x.AttractionMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.XReferralCount)).Sum(),
                    SiteReferralCount = g.SelectMany(x => x.AttractionMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.SiteReferralCount)).Sum(),

                }).ToListAsync();
            foreach (var report in reports)
            {
                report.ReportId = Guid.NewGuid().ToString();
                report.ReportPeriod = ReportPeriod.Daily;
                report.ReportLabel = dateTime.ToString("dd/MM/yyyy");
                report.CreatedAt = dateTime;
                await _unitOfWork.AttractionReportRepository.CreateAsync(report);
            }
        }
        public async Task GenerateDailyPostReport(DateTime dateTime)
        {
            DateTime startTime = dateTime.Date;
            DateTime endTime = startTime.AddHours(23).AddMinutes(59);
            var reports = await _unitOfWork.PostRepository.Query()
                .Where(x=>x.PostMetrics.Any(x=>x.CreatedAt >= startTime && x.CreatedAt <= endTime) ||
                    x.SocialMediaPosts.Any(x=>x.FacebookPostMetrics.Any(y=>y.CreatedAt >= startTime && y.CreatedAt <= endTime) || x.TwitterPostMetrics.Any(y => y.CreatedAt >= startTime && y.CreatedAt <= endTime)))
                .GroupBy(x => new { x.ProvinceId, x.PostCategoryId })
                .Select(g => new PostReport
                {
                    ProvinceId = g.Key.ProvinceId,
                    PostCategoryId = g.Key.PostCategoryId,
                    FacebookAngerCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.AngerCount),
                    FacebookCommentCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.CommentCount),
                    FacebookHahaCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.HahaCount),
                    FacebookImpressionCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ImpressionCount),
                    FacebookLikeCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LikeCount),
                    FacebookLoveCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LoveCount),
                    FacebookShareCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ShareCount),
                    FacebookWowCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.WowCount),
                    FacebookSorryCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.SorryCount),
                    XBookmarkCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.BookmarkCount),
                    XImpressionCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ImpressionCount),
                    XLikeCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LikeCount),
                    XQuoteCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.QuoteCount),
                    XReplyCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ReplyCount),
                    XRetweetCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.RetweetCount),
                    FacebookClickCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.PostClickCount),
                    SiteLikeCount = g.SelectMany(x => x.PostLikes.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Count(),
                    FacebookReferralCount = g.SelectMany(x => x.PostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.FacebookReferralCount)).Sum(),
                    XReferralCount = g.SelectMany(x => x.PostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.XReferralCount)).Sum(),
                    SiteReferralCount = g.SelectMany(x => x.PostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.SiteReferralCount)).Sum(),

                }).ToListAsync();
            foreach (var report in reports)
            {
                report.ReportId = Guid.NewGuid().ToString();
                report.ReportPeriod = ReportPeriod.Daily;
                report.ReportLabel = dateTime.ToString("dd/MM/yyyy");
                report.CreatedAt = dateTime;
                await _unitOfWork.PostReportRepository.CreateAsync(report);
            }
        }
        public async Task GenerateDailyTourTemplateReport(DateTime dateTime)
        {
            DateTime startTime = dateTime.Date;
            DateTime endTime = startTime.AddHours(23).AddMinutes(59);
            var reports = await _unitOfWork.TourTemplateRepository.Query()
                .Where(x => x.TourTemplateMetrics.Any(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime) ||
                    x.SocialMediaPosts.Any(x => x.FacebookPostMetrics.Any(y => y.CreatedAt >= startTime && y.CreatedAt <= endTime) || x.TwitterPostMetrics.Any(y => y.CreatedAt >= startTime && y.CreatedAt <= endTime)))
                .GroupBy(x => x.TourCategoryId )
                .Select(g => new
                {
                    TourCategoryId = g.Key,
                    FacebookAngerCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.AngerCount),
                    FacebookCommentCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.CommentCount),
                    FacebookHahaCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.HahaCount),
                    FacebookImpressionCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ImpressionCount),
                    FacebookLikeCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LikeCount),
                    FacebookLoveCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LoveCount),
                    FacebookShareCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ShareCount),
                    FacebookWowCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.WowCount),
                    FacebookSorryCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.SorryCount),
                    XBookmarkCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.BookmarkCount),
                    XImpressionCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ImpressionCount),
                    XLikeCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.LikeCount),
                    XQuoteCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.QuoteCount),
                    XReplyCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.ReplyCount),
                    XRetweetCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Twitter))
                        .SelectMany(x => x.TwitterPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.RetweetCount),
                    FacebookClickCount = g.SelectMany(x => x.SocialMediaPosts.Where(x => x.Site == SocialMediaSite.Facebook))
                        .SelectMany(x => x.FacebookPostMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)).Sum(x => x.PostClickCount),
                    FacebookReferralCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.FacebookReferralCount)).Sum(),
                    XReferralCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.XReferralCount)).Sum(),
                    SiteReferralCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.SiteReferralCount)).Sum(),
                    BookingCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.BookingCount)).Sum(),
                    CancellationCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.CancellationCount)).Sum(),
                    FiveStarRatingCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.FiveStarRatingCount)).Sum(),
                    FourStarRatingCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.FourStarRatingCount)).Sum(),
                    ThreeStarRatingCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.ThreeStarRatingCount)).Sum(),
                    TwoStarRatingCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.TwoStarRatingCount)).Sum(),
                    OneStarRatingCount = g.SelectMany(x => x.TourTemplateMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime).Select(x => x.OneStarRatingCount)).Sum(),
                    ProvinceIds = g.SelectMany(x => x.TourTemplateProvinces).Select(x=>x.ProvinceId).Distinct().ToList(),
                }).ToListAsync();
            foreach (var report in reports)
            {
                foreach (string provinceId in report.ProvinceIds)
                {
                    var newReport = new TourTemplateReport
                    {
                        ReportId = Guid.NewGuid().ToString(),
                        ReportPeriod = ReportPeriod.Daily,
                        ReportLabel = dateTime.ToString("dd/MM/yyyy"),
                        CreatedAt = dateTime,
                        TourCategoryId = report.TourCategoryId,
                        ProvinceId = provinceId,
                        FacebookAngerCount = report.FacebookAngerCount,
                        FacebookCommentCount = report.FacebookCommentCount,
                        FacebookHahaCount = report.FacebookHahaCount,
                        FacebookImpressionCount = report.FacebookImpressionCount,
                        FacebookLikeCount = report.FacebookLikeCount,
                        FacebookLoveCount = report.FacebookLoveCount,
                        FacebookShareCount = report.FacebookShareCount,
                        FacebookWowCount = report.FacebookWowCount,
                        FacebookSorryCount = report.FacebookSorryCount,
                        XBookmarkCount = report.XBookmarkCount,
                        XImpressionCount = report.XImpressionCount,
                        XLikeCount = report.XLikeCount,
                        XQuoteCount = report.XQuoteCount,
                        XReplyCount = report.XReplyCount,
                        XRetweetCount = report.XRetweetCount,
                        FacebookClickCount = report.FacebookClickCount,
                        SiteReferralCount = report.SiteReferralCount,
                        FacebookReferralCount = report.FacebookReferralCount,
                        XReferralCount = report.XReferralCount,
                        BookingCount = report.BookingCount,
                        CancellationCount = report.CancellationCount,
                        FiveStarRatingCount = report.FiveStarRatingCount,
                        FourStarRatingCount = report.FourStarRatingCount,
                        ThreeStarRatingCount = report.ThreeStarRatingCount,
                        TwoStarRatingCount = report.TwoStarRatingCount,
                        OneStarRatingCount = report.OneStarRatingCount,
                    };
                    await _unitOfWork.TourTemplateReportRepository.CreateAsync(newReport);
                }
            }
        }

        public async Task Test()
        {
            DateTime startDate = new DateTime(2024, 12, 1);
            DateTime endDate = new DateTime(2024, 12, 31);
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                await GenerateDailyTourTemplateReport(date);
            }
        }
    }
}
