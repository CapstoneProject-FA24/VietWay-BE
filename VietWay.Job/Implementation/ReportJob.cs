using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tweetinvi.Core.Extensions;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Job.Implementation
{
    public class ReportJob(IUnitOfWork unitOfWork, ITimeZoneHelper timeZoneHelper, IRedisCacheService redisCacheService, IIdGenerator idGenerator)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        private readonly IIdGenerator _idGenerator = idGenerator;
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
                .Where(x => x.PostMetrics.Any(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime) ||
                    x.SocialMediaPosts.Any(x => x.FacebookPostMetrics.Any(y => y.CreatedAt >= startTime && y.CreatedAt <= endTime) || x.TwitterPostMetrics.Any(y => y.CreatedAt >= startTime && y.CreatedAt <= endTime)))
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
                .GroupBy(x => x.TourCategoryId)
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
                    ProvinceIds = g.SelectMany(x => x.TourTemplateProvinces).Select(x => x.ProvinceId).Distinct().ToList(),
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
        public async Task GenerateDailyHashtagReport(DateTime dateTime)
        {
            DateTime startTime = dateTime.Date;
            DateTime endTime = startTime.AddHours(23).AddMinutes(59);
            var reports = await _unitOfWork.HashtagRepository.Query()
                .Where(x => x.SocialMediaPostHashtags.Any(x => x.SocialMediaPost.FacebookPostMetrics.Any(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime) ||
                    x.SocialMediaPost.TwitterPostMetrics.Any(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime)))
                .Select(x => new HashtagReport
                {
                    FacebookAngerCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.AngerCount),
                    FacebookClickCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.PostClickCount),
                    FacebookCommentCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.CommentCount),
                    FacebookHahaCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.HahaCount),
                    FacebookImpressionCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.ImpressionCount),
                    FacebookLikeCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.LikeCount),
                    FacebookLoveCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.LoveCount),
                    FacebookShareCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.ShareCount),
                    FacebookSorryCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.SorryCount),
                    FacebookWowCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.FacebookPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.WowCount),
                    HashtagId = x.HashtagId,
                    XBookmarkCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.TwitterPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.BookmarkCount),
                    XImpressionCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.TwitterPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.ImpressionCount),
                    XLikeCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.TwitterPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.LikeCount),
                    XQuoteCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.TwitterPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.QuoteCount),
                    XReplyCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.TwitterPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.ReplyCount),
                    XRetweetCount = x.SocialMediaPostHashtags
                        .SelectMany(x => x.SocialMediaPost.TwitterPostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.RetweetCount),
                    FacebookReferralCount = x.SocialMediaPostHashtags
                        .Where(x => x.SocialMediaPost.Site == SocialMediaSite.Facebook)
                        .SelectMany(x => x.SocialMediaPost.Attraction.AttractionMetrics.Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.FacebookReferralCount) +
                        x.SocialMediaPostHashtags
                        .Where(x => x.SocialMediaPost.Site == SocialMediaSite.Facebook)
                        .SelectMany(x => x.SocialMediaPost.Post.PostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.FacebookReferralCount) +
                        x.SocialMediaPostHashtags
                        .Where(x => x.SocialMediaPost.Site == SocialMediaSite.Facebook)
                        .SelectMany(x => x.SocialMediaPost.TourTemplate.TourTemplateMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.FacebookReferralCount),
                    XReferralCount = x.SocialMediaPostHashtags
                        .Where(x => x.SocialMediaPost.Site == SocialMediaSite.Twitter)
                        .SelectMany(x => x.SocialMediaPost.Attraction.AttractionMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.XReferralCount) +
                        x.SocialMediaPostHashtags
                        .Where(x => x.SocialMediaPost.Site == SocialMediaSite.Twitter)
                        .SelectMany(x => x.SocialMediaPost.Post.PostMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.XReferralCount) +
                        x.SocialMediaPostHashtags
                        .Where(x => x.SocialMediaPost.Site == SocialMediaSite.Twitter)
                        .SelectMany(x => x.SocialMediaPost.TourTemplate.TourTemplateMetrics
                        .Where(x => x.CreatedAt >= startTime && x.CreatedAt <= endTime))
                        .Sum(x => x.XReferralCount),
                }).ToListAsync();
            foreach (var report in reports)
            {
                report.ReportId = Guid.NewGuid().ToString();
                report.ReportPeriod = ReportPeriod.Daily;
                report.ReportLabel = dateTime.ToString("dd/MM/yyyy");
                report.CreatedAt = dateTime;
                await _unitOfWork.HashtagReportRepository.CreateAsync(report);
            }
        }

        public async Task GetPostMetrics(DateTime dateTime)
        {
            Dictionary<string, PostMetric> postMetrics = new Dictionary<string, PostMetric>();
            List<string> referrences = await _redisCacheService.GetMultipleKeyAsync($"*Referrence-{SocialMediaPostEntity.Post}-*");
            foreach (string referrence in referrences)
            {
                string[] referrenceParts = referrence.Split("-");
                string platform = referrenceParts[0];
                string referrenceType = referrenceParts[1];
                string referrenceId = referrenceParts[2];
                switch (platform)
                {
                    case "facebookReferrence":
                        if (!postMetrics.ContainsKey(referrenceId))
                        {
                            postMetrics.Add(referrenceId, new PostMetric());
                        }
                        postMetrics[referrenceId].FacebookReferralCount = await _redisCacheService.GetAsync<int>($"facebookReferrence-{SocialMediaPostEntity.Post}-{referrenceId}");
                        break;
                    case "twitterReferrence":
                        if (!postMetrics.ContainsKey(referrenceId))
                        {
                            postMetrics.Add(referrenceId, new PostMetric());
                        }
                        postMetrics[referrenceId].XReferralCount = await _redisCacheService.GetAsync<int>($"twitterReferrence-{SocialMediaPostEntity.Post}-{referrenceId}");
                        break;
                    default:
                        if (!postMetrics.ContainsKey(referrenceId))
                        {
                            postMetrics.Add(referrenceId, new PostMetric());
                        }
                        postMetrics[referrenceId].SiteReferralCount = await _redisCacheService.GetAsync<int>($"siteReferrence-{SocialMediaPostEntity.Post}-{referrenceId}");
                        break;
                };
            }
            var postLikes = await _unitOfWork.PostLikeRepository.Query()
                .Where(x => x.CreatedAt >= dateTime.Date && x.CreatedAt <= dateTime.Date.AddHours(23).AddMinutes(59))
                .GroupBy(x => x.PostId)
                .Select(g => new
                {
                    PostId = g.Key,
                    LikeCount = g.Count(),
                }).ToListAsync();
            foreach (var postLike in postLikes)
            {
                if (!postMetrics.ContainsKey(postLike.PostId))
                {
                    postMetrics.Add(postLike.PostId, new PostMetric());
                }
                postMetrics[postLike.PostId].SiteSaveCount = postLike.LikeCount;
            }
            foreach (var postMetric in postMetrics)
            {
                try
                {
                    PostMetric metric = postMetric.Value;
                    metric.CreatedAt = dateTime;
                    metric.PostId = postMetric.Key;
                    metric.MetricId = _idGenerator.GenerateId();
                    await _unitOfWork.PostMetricRepository.CreateAsync(metric);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public async Task GetAttractionMetrics(DateTime dateTime)
        {
            Dictionary<string, AttractionMetric> attractionMetrics = new Dictionary<string, AttractionMetric>();
            List<string> referrences = await _redisCacheService.GetMultipleKeyAsync($"*Referrence-{SocialMediaPostEntity.Attraction}-*");
            foreach (string referrence in referrences)
            {
                Console.WriteLine(referrence);
                string[] referrenceParts = referrence.Split("-");
                string platform = referrenceParts[0];
                string referrenceType = referrenceParts[1];
                string referrenceId = referrenceParts[2];
                switch (platform)
                {
                    case "facebookReferrence":
                        if (!attractionMetrics.ContainsKey(referrenceId))
                        {
                            attractionMetrics.Add(referrenceId, new AttractionMetric());
                        }
                        attractionMetrics[referrenceId].FacebookReferralCount = await _redisCacheService.GetAsync<int>($"facebookReferrence-{SocialMediaPostEntity.Attraction}-{referrenceId}");
                        break;
                    case "twitterReferrence":
                        if (!attractionMetrics.ContainsKey(referrenceId))
                        {
                            attractionMetrics.Add(referrenceId, new AttractionMetric());
                        }
                        attractionMetrics[referrenceId].XReferralCount = await _redisCacheService.GetAsync<int>($"twitterReferrence-{SocialMediaPostEntity.Attraction}-{referrenceId}");
                        break;
                    default:
                        if (!attractionMetrics.ContainsKey(referrenceId))
                        {
                            attractionMetrics.Add(referrenceId, new AttractionMetric());
                        }
                        attractionMetrics[referrenceId].SiteReferralCount = await _redisCacheService.GetAsync<int>($"siteReferrence-{SocialMediaPostEntity.Attraction}-{referrenceId}");
                        break;
                };
            }
            var attractionLikes = await _unitOfWork.AttractionLikeRepository.Query()
                .Where(x => x.CreatedAt >= dateTime.Date && x.CreatedAt <= dateTime.Date.AddHours(23).AddMinutes(59))
                .GroupBy(x => x.AttractionId)
                .Select(g => new
                {
                    AttractionId = g.Key,
                    LikeCount = g.Count(),
                }).ToListAsync();
            foreach (var attractionLike in attractionLikes)
            {
                if (!attractionMetrics.ContainsKey(attractionLike.AttractionId))
                {
                    attractionMetrics.Add(attractionLike.AttractionId, new AttractionMetric());
                }
                attractionMetrics[attractionLike.AttractionId].SiteLikeCount = attractionLike.LikeCount;
            }
            var attractionReviews = await _unitOfWork.AttractionReviewRepository.Query()
                .Where(x => x.CreatedAt >= dateTime.Date && x.CreatedAt <= dateTime.Date.AddHours(23).AddMinutes(59))
                .GroupBy(x => x.AttractionId)
                .Select(g => new
                {
                    AttractionId = g.Key,
                    FiveStarRatingCount = g.Count(x => x.Rating == 5),
                    FourStarRatingCount = g.Count(x => x.Rating == 4),
                    ThreeStarRatingCount = g.Count(x => x.Rating == 3),
                    TwoStarRatingCount = g.Count(x => x.Rating == 2),
                    OneStarRatingCount = g.Count(x => x.Rating == 1),
                }).ToListAsync();
            foreach (var attractionReview in attractionReviews)
            {
                if (!attractionMetrics.ContainsKey(attractionReview.AttractionId))
                {
                    attractionMetrics.Add(attractionReview.AttractionId, new AttractionMetric());
                }
                attractionMetrics[attractionReview.AttractionId].FiveStarRatingCount = attractionReview.FiveStarRatingCount;
                attractionMetrics[attractionReview.AttractionId].FourStarRatingCount = attractionReview.FourStarRatingCount;
                attractionMetrics[attractionReview.AttractionId].ThreeStarRatingCount = attractionReview.ThreeStarRatingCount;
                attractionMetrics[attractionReview.AttractionId].TwoStarRatingCount = attractionReview.TwoStarRatingCount;
                attractionMetrics[attractionReview.AttractionId].OneStarRatingCount = attractionReview.OneStarRatingCount;
            }
            var attractionReviewLikes = await _unitOfWork.AttractionReviewLikeRepository.Query()
                .Where(x => x.CreatedAt >= dateTime.Date && x.CreatedAt <= dateTime.Date.AddHours(23).AddMinutes(59))
                .GroupBy(x => x.AttractionReview.Attraction.AttractionId)
                .Select(g => new
                {
                    g.Key,
                    FiveStarRatingLikeCount = g.Count(x => x.AttractionReview.Rating == 5),
                    FourStarRatingLikeCount = g.Count(x => x.AttractionReview.Rating == 4),
                    ThreeStarRatingLikeCount = g.Count(x => x.AttractionReview.Rating == 3),
                    TwoStarRatingLikeCount = g.Count(x => x.AttractionReview.Rating == 2),
                    OneStarRatingLikeCount = g.Count(x => x.AttractionReview.Rating == 1),
                }).ToListAsync();
            foreach (var attractionReviewLike in attractionReviewLikes)
            {
                if (!attractionMetrics.ContainsKey(attractionReviewLike.Key))
                {
                    attractionMetrics.Add(attractionReviewLike.Key, new AttractionMetric());
                }
                attractionMetrics[attractionReviewLike.Key].FiveStarRatingLikeCount = attractionReviewLike.FiveStarRatingLikeCount;
                attractionMetrics[attractionReviewLike.Key].FourStarRatingLikeCount = attractionReviewLike.FourStarRatingLikeCount;
                attractionMetrics[attractionReviewLike.Key].ThreeStarRatingLikeCount = attractionReviewLike.ThreeStarRatingLikeCount;
                attractionMetrics[attractionReviewLike.Key].TwoStarRatingLikeCount = attractionReviewLike.TwoStarRatingLikeCount;
                attractionMetrics[attractionReviewLike.Key].OneStarRatingLikeCount = attractionReviewLike.OneStarRatingLikeCount;
            }
            foreach (var attractionMetric in attractionMetrics)
            {
                try
                {
                    AttractionMetric metric = attractionMetric.Value;
                    metric.CreatedAt = dateTime;
                    metric.AttractionId = attractionMetric.Key;
                    metric.MetricId = _idGenerator.GenerateId();
                    await _unitOfWork.AttractionMetricRepository.CreateAsync(metric);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task GetTourTemplateMetrics(DateTime dateTime)
        {
            Dictionary<string, TourTemplateMetric> tourTemplateMetrics = new Dictionary<string, TourTemplateMetric>();
            List<string> referrences = await _redisCacheService.GetMultipleKeyAsync($"*Referrence-{SocialMediaPostEntity.TourTemplate}-*");
            foreach (string referrence in referrences)
            {
                string[] referrenceParts = referrence.Split("-");
                string platform = referrenceParts[0];
                string referrenceType = referrenceParts[1];
                string referrenceId = referrenceParts[2];
                switch (platform)
                {
                    case "facebookReferrence":
                        if (!tourTemplateMetrics.ContainsKey(referrenceId))
                        {
                            tourTemplateMetrics.Add(referrenceId, new TourTemplateMetric());
                        }
                        tourTemplateMetrics[referrenceId].FacebookReferralCount = await _redisCacheService.GetAsync<int>($"facebookReferrence-{SocialMediaPostEntity.TourTemplate}-{referrenceId}");
                        break;
                    case "twitterReferrence":
                        if (!tourTemplateMetrics.ContainsKey(referrenceId))
                        {
                            tourTemplateMetrics.Add(referrenceId, new TourTemplateMetric());
                        }
                        tourTemplateMetrics[referrenceId].XReferralCount = await _redisCacheService.GetAsync<int>($"twitterReferrence-{SocialMediaPostEntity.TourTemplate}-{referrenceId}");
                        break;
                    default:
                        if (!tourTemplateMetrics.ContainsKey(referrenceId))
                        {
                            tourTemplateMetrics.Add(referrenceId, new TourTemplateMetric());
                        }
                        tourTemplateMetrics[referrenceId].SiteReferralCount = await _redisCacheService.GetAsync<int>($"siteReferrence-{SocialMediaPostEntity.TourTemplate}-{referrenceId}");
                        break;
                };
            }
            var tourTemplateBookings = await _unitOfWork.BookingRepository.Query()
                .Where(x => x.CreatedAt >= dateTime.Date && x.CreatedAt <= dateTime.Date.AddHours(23).AddMinutes(59))
                .GroupBy(x => x.Tour.TourTemplateId)
                .Select(g => new
                {
                    TourTemplateId = g.Key,
                    BookingCount = g.Count(),
                }).ToListAsync();
            foreach (var tourTemplateBooking in tourTemplateBookings)
            {
                if (!tourTemplateMetrics.ContainsKey(tourTemplateBooking.TourTemplateId))
                {
                    tourTemplateMetrics.Add(tourTemplateBooking.TourTemplateId, new TourTemplateMetric());
                }
                tourTemplateMetrics[tourTemplateBooking.TourTemplateId].BookingCount = tourTemplateBooking.BookingCount;
            }
            var tourTemplateCancellations = await _unitOfWork.BookingRefundRepository.Query()
                .Where(x => x.CreatedAt >= dateTime.Date && x.CreatedAt <= dateTime.Date.AddHours(23).AddMinutes(59))
                .GroupBy(x => x.Booking.Tour.TourTemplateId)
                .Select(g => new
                {
                    TourTemplateId = g.Key,
                    CancellationCount = g.Count(),
                }).ToListAsync();
            foreach (var tourTemplateCancellation in tourTemplateCancellations)
            {
                if (!tourTemplateMetrics.ContainsKey(tourTemplateCancellation.TourTemplateId))
                {
                    tourTemplateMetrics.Add(tourTemplateCancellation.TourTemplateId, new TourTemplateMetric());
                }
                tourTemplateMetrics[tourTemplateCancellation.TourTemplateId].CancellationCount = tourTemplateCancellation.CancellationCount;
            }
            var tourTemplateReviews = await _unitOfWork.TourReviewRepository.Query()
                .Where(x => x.CreatedAt >= dateTime.Date && x.CreatedAt <= dateTime.Date.AddHours(23).AddMinutes(59))
                .GroupBy(x => x.Booking.Tour.TourTemplateId)
                .Select(g => new
                {
                    TourTemplateId = g.Key,
                    FiveStarRatingCount = g.Count(x => x.Rating == 5),
                    FourStarRatingCount = g.Count(x => x.Rating == 4),
                    ThreeStarRatingCount = g.Count(x => x.Rating == 3),
                    TwoStarRatingCount = g.Count(x => x.Rating == 2),
                    OneStarRatingCount = g.Count(x => x.Rating == 1),
                }).ToListAsync();
            foreach (var tourTemplateReview in tourTemplateReviews)
            {
                if (!tourTemplateMetrics.ContainsKey(tourTemplateReview.TourTemplateId))
                {
                    tourTemplateMetrics.Add(tourTemplateReview.TourTemplateId, new TourTemplateMetric());
                }
                tourTemplateMetrics[tourTemplateReview.TourTemplateId].FiveStarRatingCount = tourTemplateReview.FiveStarRatingCount;
                tourTemplateMetrics[tourTemplateReview.TourTemplateId].FourStarRatingCount = tourTemplateReview.FourStarRatingCount;
                tourTemplateMetrics[tourTemplateReview.TourTemplateId].ThreeStarRatingCount = tourTemplateReview.ThreeStarRatingCount;
                tourTemplateMetrics[tourTemplateReview.TourTemplateId].TwoStarRatingCount = tourTemplateReview.TwoStarRatingCount;
                tourTemplateMetrics[tourTemplateReview.TourTemplateId].OneStarRatingCount = tourTemplateReview.OneStarRatingCount;
            }
            foreach (var tourTemplateMetric in tourTemplateMetrics)
            {
                try
                {
                    TourTemplateMetric metric = tourTemplateMetric.Value;
                    metric.CreatedAt = dateTime;
                    metric.TourTemplateId = tourTemplateMetric.Key;
                    metric.MetricId = _idGenerator.GenerateId();
                    await _unitOfWork.TourTemplateMetricRepository.CreateAsync(metric);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task GenerateReport(int tryNo = 0)
        {
            DateTime date = _timeZoneHelper.GetUTC7Now();
            bool tourMetric = (await _redisCacheService.GetAsync<string>("tourMetric")).IsNullOrEmpty();
            if (tourMetric == true)
            {
                await GetTourTemplateMetrics(date);
                await _redisCacheService.SetAsync("tourMetric", "false", TimeSpan.FromHours(20));
            }
            bool attractionMetric = (await _redisCacheService.GetAsync<string>("attractionMetric")).IsNullOrEmpty();
            if (attractionMetric == true)
            {
                await GetAttractionMetrics(date);
                await _redisCacheService.SetAsync("attractionMetric", "false", TimeSpan.FromHours(20));
            }
            bool postMetric = (await _redisCacheService.GetAsync<string>("postMetric")).IsNullOrEmpty();
            if (postMetric == true)
            {
                await GetPostMetrics(date);
                await _redisCacheService.SetAsync("postMetric", "false", TimeSpan.FromHours(20));
            }
            bool tourReport = (await _redisCacheService.GetAsync<string>("tourReport")).IsNullOrEmpty();
            if (tourReport == true)
            {
                await GenerateDailyTourTemplateReport(date);
                await _redisCacheService.SetAsync("tourReport", "false", TimeSpan.FromHours(20));
            }
            bool attractionReport = (await _redisCacheService.GetAsync<string>("attractionReport")).IsNullOrEmpty();
            if (attractionReport == true)
            {
                await GenerateDailyAttractionReport(date);
                await _redisCacheService.SetAsync("attractionReport", "false", TimeSpan.FromHours(20));
            }
            bool postReport = (await _redisCacheService.GetAsync<string>("postReport")).IsNullOrEmpty();
            if (postReport == true)
            {
                await GenerateDailyPostReport(date);
                await _redisCacheService.SetAsync("postReport", "false", TimeSpan.FromHours(20));
            }
        }
    }
}