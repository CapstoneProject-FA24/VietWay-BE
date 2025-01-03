using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VietWay.Service.Management.Implement
{
    public class ReportService(IUnitOfWork unitOfWork) : IReportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ReportBookingDTO> GetReportBookingAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidInfoException("INVALID_INFO_START_DATE_AFTER_END_DATE");
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            int totalBooking = await _unitOfWork.BookingRepository.Query().CountAsync(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate);

            ReportBookingByDay reportBookingByDay = new()
            {
                Dates = GetPeriodLabels(startDate, endDate),
                CancelledBookings = [],
                CompletedBookings = [],
                DepositedBookings = [],
                PaidBookings = [],
                PendingBookings = []
            };
            switch (GetPeriod(startDate, endDate))
            {
                case ReportPeriod.Daily:
                    for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                    {
                        reportBookingByDay.PendingBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1) && x.Status == BookingStatus.Pending));
                        reportBookingByDay.CancelledBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1) && x.Status == BookingStatus.Cancelled));
                        reportBookingByDay.CompletedBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1) && x.Status == BookingStatus.Completed));
                        reportBookingByDay.DepositedBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1) && x.Status == BookingStatus.Deposited));
                        reportBookingByDay.PaidBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1) && x.Status == BookingStatus.Paid));
                    }
                    break;

                case ReportPeriod.Monthly:
                    DateTime monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                    DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
                    for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                    {
                        reportBookingByDay.PendingBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1) && x.Status == BookingStatus.Pending));
                        reportBookingByDay.CancelledBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1) && x.Status == BookingStatus.Cancelled));
                        reportBookingByDay.CompletedBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1) && x.Status == BookingStatus.Completed));
                        reportBookingByDay.DepositedBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1) && x.Status == BookingStatus.Deposited));
                        reportBookingByDay.PaidBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1) && x.Status == BookingStatus.Paid));
                    }
                    break;

                case ReportPeriod.Quarterly:
                    DateTime quarterStart = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                    DateTime quarterEnd = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1);
                    for (DateTime date = quarterStart; date <= quarterEnd; date = date.AddMonths(3))
                    {
                        reportBookingByDay.PendingBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3) && x.Status == BookingStatus.Pending));
                        reportBookingByDay.CancelledBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3) && x.Status == BookingStatus.Cancelled));
                        reportBookingByDay.CompletedBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3) && x.Status == BookingStatus.Completed));
                        reportBookingByDay.DepositedBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3) && x.Status == BookingStatus.Deposited));
                        reportBookingByDay.PaidBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                            .CountAsync(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3) && x.Status == BookingStatus.Paid));
                    }
                    break;

                case ReportPeriod.Yearly:
                    for (int year = startDate.Year; year <= endDate.Year; year++)
                    {
                        reportBookingByDay.PendingBookings.Add(
                        await _unitOfWork.BookingRepository.Query()
                        .CountAsync(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1 && x.Status == BookingStatus.Pending));
                        reportBookingByDay.CancelledBookings.Add(
                        await _unitOfWork.BookingRepository.Query()
                        .CountAsync(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1 && x.Status == BookingStatus.Cancelled));
                        reportBookingByDay.CompletedBookings.Add(
                        await _unitOfWork.BookingRepository.Query()
                        .CountAsync(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1 && x.Status == BookingStatus.Completed));
                        reportBookingByDay.DepositedBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                        .CountAsync(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1 && x.Status == BookingStatus.Deposited));
                        reportBookingByDay.PaidBookings.Add(
                            await _unitOfWork.BookingRepository.Query()
                        .CountAsync(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1 && x.Status == BookingStatus.Paid));
                    }
                    break;
            }
            List<ReportBookingParticipantCountDTO> participantCounts = await _unitOfWork.BookingRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .GroupBy(x => x.BookingTourists.Count)
                .Select(x => new ReportBookingParticipantCountDTO
                {
                    ParticipantCount = x.Key,
                    BookingCount = x.Count()
                })
                .OrderByDescending(x => x.BookingCount)
                .ToListAsync();
            List<ReportBookingByTourCategory> categoryBookings = await _unitOfWork.TourCategoryRepository.Query()
                .Select(x => new ReportBookingByTourCategory
                {
                    TourCategoryName = x.Name,
                    TotalBooking = x.TourTemplates.SelectMany(t => t.Tours).SelectMany(t => t.TourBookings)
                        .Where(b => b.CreatedAt >= startDate && b.CreatedAt <= endDate).Count()
                })
                .OrderByDescending(x => x.TotalBooking)
                .ToListAsync();
            List<ReportBookingByTourTemplate> templateBookings = await _unitOfWork.TourTemplateRepository.Query()
                .Select(x => new ReportBookingByTourTemplate
                {
                    TourTemplateName = x.TourName,
                    TotalBooking = x.Tours.SelectMany(t => t.TourBookings)
                        .Where(b => b.CreatedAt >= startDate && b.CreatedAt <= endDate).Count()
                })
                .OrderByDescending(x => x.TotalBooking)
                .ToListAsync();
            return new ReportBookingDTO
            {
                TotalBooking = totalBooking,
                ReportBookingByDay = reportBookingByDay,
                ReportBookingByParticipantCount = participantCounts,
                ReportBookingByTourCategory = categoryBookings,
                ReportBookingByTourTemplate = templateBookings
            };
        }

        public async Task<ReportRatingDTO> GetReportRatingAsync(DateTime startDate, DateTime endDate, bool isAsc)
        {
            if (startDate > endDate)
            {
                throw new InvalidInfoException("INVALID_INFO_START_DATE_AFTER_END_DATE");
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            IQueryable<AttractionRatingDTO> attractions = _unitOfWork.AttractionRepository.Query()
                .Where(x => x.Status == AttractionStatus.Approved)
                .Select(x => new AttractionRatingDTO
                {
                    AttractionName = x.Name,
                    AverageRating = x.AttractionReviews.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).Select(r => r.Rating).DefaultIfEmpty().Average(),
                    TotalRating = x.AttractionReviews.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).Count()
                });
            IQueryable<TourTemplateRatingDTO> tourTemplates = _unitOfWork.TourTemplateRepository.Query()
                .Where(x => x.Status == TourTemplateStatus.Approved)
                .Select(x => new TourTemplateRatingDTO
                {
                    TourTemplateName = x.TourName,
                    AverageRating = x.Tours.SelectMany(t => t.TourBookings).Select(x => x.TourReview).Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).Select(r => r.Rating).DefaultIfEmpty().Average(),
                    TotalRating = x.Tours.SelectMany(t => t.TourBookings).Select(x => x.TourReview).Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).Count()
                });
            attractions = isAsc ? attractions.OrderBy(x => x.AverageRating) : attractions.OrderByDescending(x => x.AverageRating);
            tourTemplates = isAsc ? tourTemplates.OrderBy(x => x.AverageRating) : tourTemplates.OrderByDescending(x => x.AverageRating);

            IQueryable<AttractionRatingDTO> attractionTotal = _unitOfWork.AttractionRepository.Query()
                .Where(x => x.Status == AttractionStatus.Approved)
                .Select(x => new AttractionRatingDTO
                {
                    AttractionName = x.Name,
                    AverageRating = x.AttractionReviews.Select(r => r.Rating).DefaultIfEmpty().Average(),
                    TotalRating = x.AttractionReviews.Count()
                });
            IQueryable<TourTemplateRatingDTO> tourTemplateTotal = _unitOfWork.TourTemplateRepository.Query()
                .Where(x => x.Status == TourTemplateStatus.Approved)
                .Select(x => new TourTemplateRatingDTO
                {
                    TourTemplateName = x.TourName,
                    AverageRating = x.Tours.SelectMany(t => t.TourBookings).Select(x => x.TourReview.Rating).DefaultIfEmpty().Average(),
                    TotalRating = x.Tours.SelectMany(t => t.TourBookings).Select(x => x.TourReview).Count()
                });
            attractionTotal = isAsc ? attractions.OrderBy(x => x.AverageRating) : attractions.OrderByDescending(x => x.AverageRating);
            tourTemplateTotal = isAsc ? tourTemplates.OrderBy(x => x.AverageRating) : tourTemplates.OrderByDescending(x => x.AverageRating);

            var attractionRatingInPeriod = await attractions.Take(15).ToListAsync();
            var attractionRatingTotal = await attractionTotal.Take(15).ToListAsync();
            var tourTemplateRatingInPeriod = await tourTemplates.Take(15).ToListAsync();
            var tourTemplateRatingTotal = await tourTemplateTotal.Take(15).ToListAsync();
            return new ReportRatingDTO
            {
                AttractionRatingInPeriod = attractionRatingInPeriod,
                AttractionRatingTotal = attractionRatingTotal,
                TourTemplateRatingInPeriod = tourTemplateRatingInPeriod,
                TourTemplateRatingTotal = tourTemplateRatingTotal
            };
        }

        public async Task<ReportRevenueDTO> GetReportRevenueAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new InvalidInfoException("INVALID_INFO_START_DATE_AFTER_END_DATE");
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            List<string> periods = GetPeriodLabels(startDate, endDate);
            List<decimal> revenueByPeriods = [];
            List<decimal> refundByPeriods = [];

            switch (GetPeriod(startDate, endDate))
            {
                case ReportPeriod.Daily:
                    for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                    {
                        revenueByPeriods.Add(await _unitOfWork.BookingRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                            .SumAsync(x => x.PaidAmount));
                        refundByPeriods.Add(await _unitOfWork.BookingRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                            .SelectMany(x => x.BookingRefunds.Where(y => y.RefundStatus == RefundStatus.Refunded))
                            .SumAsync(x => x.RefundAmount));
                    }
                    break;

                case ReportPeriod.Monthly:
                    DateTime monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                    DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1); // Correct end date to last day of month
                    for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                    {
                        revenueByPeriods.Add(await _unitOfWork.BookingRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt <= date.AddMonths(1))
                            .SumAsync(x => x.PaidAmount));
                        refundByPeriods.Add(await _unitOfWork.BookingRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt <= date.AddMonths(1))
                            .SelectMany(x => x.BookingRefunds.Where(y => y.RefundStatus == RefundStatus.Refunded))
                            .SumAsync(x => x.RefundAmount));
                    }
                    break;

                case ReportPeriod.Quarterly:
                    DateTime quarterStart = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                    DateTime quarterEnd = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1);
                    for (DateTime date = quarterStart; date <= quarterEnd; date = date.AddMonths(3))
                    {
                        revenueByPeriods.Add(await _unitOfWork.BookingRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                            .SumAsync(x => x.PaidAmount));
                        refundByPeriods.Add(await _unitOfWork.BookingRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                            .SelectMany(x => x.BookingRefunds.Where(y => y.RefundStatus == RefundStatus.Refunded))
                            .SumAsync(x => x.RefundAmount));
                    }
                    break;

                case ReportPeriod.Yearly:
                    for (int year = startDate.Year; year <= endDate.Year; year++)
                    {
                        revenueByPeriods.Add(await _unitOfWork.BookingRepository.Query()
                            .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                            .SumAsync(x => x.PaidAmount));
                        refundByPeriods.Add(await _unitOfWork.BookingRepository.Query()
                            .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                            .SelectMany(x => x.BookingRefunds.Where(y => y.RefundStatus == RefundStatus.Refunded))
                            .SumAsync(x => x.RefundAmount));
                    }
                    break;
            }
            decimal refunds = await _unitOfWork.BookingRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .SelectMany(x => x.BookingRefunds.Where(y => y.RefundStatus == RefundStatus.Refunded))
                .SumAsync(x => x.RefundAmount);
            decimal totalRevenue = await _unitOfWork.BookingRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .SumAsync(x => x.PaidAmount) - refunds;
            List<ReportRevenueByTourCategory> categoryRevenue = await _unitOfWork.TourCategoryRepository.Query()
                .Select(x => new ReportRevenueByTourCategory
                {
                    TourCategoryName = x.Name,
                    TotalRevenue = x.TourTemplates.SelectMany(t => t.Tours).SelectMany(t => t.TourBookings)
                        .Where(b => b.CreatedAt >= startDate && b.CreatedAt <= endDate).Sum(b => b.PaidAmount) -
                        x.TourTemplates.SelectMany(t => t.Tours).SelectMany(t => t.TourBookings).SelectMany(t => t.BookingRefunds)
                        .Where(x => x.RefundStatus == RefundStatus.Refunded).Sum(x => x.RefundAmount)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToListAsync();
            List<ReportRevenueByTourTemplate> templateRevenue = await _unitOfWork.TourTemplateRepository.Query()
                .Select(x => new ReportRevenueByTourTemplate
                {
                    TourTemplateName = x.TourName,
                    TotalRevenue = x.Tours.SelectMany(t => t.TourBookings).
                    Where(b => b.CreatedAt >= startDate && b.CreatedAt <= endDate).Sum(b => b.PaidAmount) -
                    x.Tours.SelectMany(t => t.TourBookings).SelectMany(t => t.BookingRefunds)
                    .Where(x => x.RefundStatus == RefundStatus.Refunded).Sum(x => x.RefundAmount)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToListAsync();
            return new ReportRevenueDTO
            {
                TotalRevenue = totalRevenue,
                ReportRevenueByPeriod = new ReportRevenueByPeriod
                {
                    Periods = periods,
                    Revenue = revenueByPeriods,
                    Refund = refundByPeriods
                },
                ReportRevenueByTourCategory = categoryRevenue,
                ReportRevenueByTourTemplate = templateRevenue
            };
        }

        public async Task<ReportSummaryDTO> GetReportSummaryAsync(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            double averageTourRating = await _unitOfWork.TourReviewRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .AverageAsync(x => x.Rating);
            int attractionCount = await _unitOfWork.AttractionRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .CountAsync();
            int bookingCount = await _unitOfWork.BookingRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .CountAsync();
            int customerCount = await _unitOfWork.AccountRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.Role == UserRole.Customer)
                .CountAsync();
            int postCount = await _unitOfWork.PostRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.Status == PostStatus.Approved)
                .CountAsync();
            int tourCount = await _unitOfWork.TourRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.Status != TourStatus.Pending || x.Status != TourStatus.Rejected)
                .CountAsync();
            decimal profit = await _unitOfWork.BookingRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .SumAsync(x => x.PaidAmount);
            decimal lost = await _unitOfWork.BookingRefundRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.RefundStatus == RefundStatus.Refunded)
                .SumAsync(x => x.RefundAmount);
            return new ReportSummaryDTO()
            {
                AverateTourRating = (decimal)averageTourRating,
                NewAttraction = attractionCount,
                NewBooking = bookingCount,
                NewCustomer = customerCount,
                NewPost = postCount,
                NewTour = tourCount,
                Revenue = profit - lost
            };
        }

        public async Task<ReportPromotionSummaryDTO> GetPromotionSummaryAsync(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            var facebookPosts = await _unitOfWork.FacebookPostMetricRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalPosts = g.Count(),
                    TotalComments = g.Sum(x => x.CommentCount),
                    TotalImpressions = g.Sum(x => x.ImpressionCount),
                    TotalShares = g.Sum(x => x.ShareCount),
                    TotalReactions = g.Sum(x => x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount),
                    TotalScore = g.Sum(x => x.Score)
                })
                .FirstAsync();
            var xPosts = await _unitOfWork.TwitterPostMetricRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalPosts = g.Count(),
                    TotalImpressions = g.Sum(x => x.ImpressionCount),
                    TotalLikes = g.Sum(x => x.LikeCount),
                    TotalReplies = g.Sum(x => x.ReplyCount),
                    TotalRetweets = g.Sum(x => x.RetweetCount + x.QuoteCount),
                    TotalScore = g.Sum(x => x.Score)
                })
                .FirstAsync();
            return new ReportPromotionSummaryDTO
            {
                TotalFacebookPost = facebookPosts.TotalPosts,
                FacebookCommentCount = facebookPosts.TotalComments ?? 0,
                FacebookImpressionCount = facebookPosts.TotalImpressions ?? 0,
                FacebookShareCount = facebookPosts.TotalShares ?? 0,
                FacebookReactionCount = facebookPosts.TotalReactions ?? 0,
                TotalXPost = xPosts.TotalPosts,
                XImpressionCount = xPosts.TotalImpressions ?? 0,
                XLikeCount = xPosts.TotalLikes ?? 0,
                XReplyCount = xPosts.TotalReplies ?? 0,
                XRetweetCount = xPosts.TotalRetweets ?? 0
            };
        }

        public async Task<ReportSocialMediaSummaryDTO> GetSocialMediaSummaryAsync(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            ReportSocialMediaSummaryDTO report = new()
            {
                Dates = GetPeriodLabels(startDate, endDate),
                FacebookComments = [],
                FacebookImpressions = [],
                FacebookReactions = [],
                FacebookShares = [],
                FacebookScore = [],
                XImpressions = [],
                XLikes = [],
                XReplies = [],
                XRetweets = [],
                XScore = []
            };
            switch (GetPeriod(startDate, endDate))
            {
                case ReportPeriod.Daily:
                    for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                    {
                        var facebookMetrics = await _unitOfWork.FacebookPostMetricRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                            .GroupBy(x => 1)
                            .Select(g => new
                            {
                                TotalComments = g.Sum(x => x.CommentCount),
                                TotalImpressions = g.Sum(x => x.ImpressionCount),
                                TotalShares = g.Sum(x => x.ShareCount),
                                TotalReactions = g.Sum(x => x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount),
                                TotalScore = g.Sum(x => x.Score)
                            })
                            .FirstOrDefaultAsync();
                        report.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                        report.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                        report.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                        report.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                        report.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                        var twitterMetrics = await _unitOfWork.TwitterPostMetricRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                            .GroupBy(x => 1)
                            .Select(g => new
                            {
                                TotalImpressions = g.Sum(x => x.ImpressionCount),
                                TotalLikes = g.Sum(x => x.LikeCount),
                                TotalReplies = g.Sum(x => x.ReplyCount),
                                TotalRetweets = g.Sum(x => x.RetweetCount + x.QuoteCount),
                                TotalScore = g.Sum(x => x.Score)
                            })
                            .FirstOrDefaultAsync();
                        report.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                        report.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                        report.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                        report.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                        report.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                    }
                    break;
                case ReportPeriod.Monthly:
                    DateTime monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                    DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
                    for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                    {
                        var facebookMetrics = await _unitOfWork.FacebookPostMetricRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                            .GroupBy(x => 1)
                            .Select(g => new
                            {
                                TotalComments = g.Sum(x => x.CommentCount),
                                TotalImpressions = g.Sum(x => x.ImpressionCount),
                                TotalShares = g.Sum(x => x.ShareCount),
                                TotalReactions = g.Sum(x => x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount),
                                TotalScore = g.Sum(x => x.Score)
                            })
                            .FirstOrDefaultAsync();
                        report.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                        report.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                        report.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                        report.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                        report.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                        var twitterMetrics = await _unitOfWork.TwitterPostMetricRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                            .GroupBy(x => 1)
                            .Select(g => new
                            {
                                TotalImpressions = g.Sum(x => x.ImpressionCount),
                                TotalLikes = g.Sum(x => x.LikeCount),
                                TotalReplies = g.Sum(x => x.ReplyCount),
                                TotalRetweets = g.Sum(x => x.RetweetCount + x.QuoteCount),
                                TotalScore = g.Sum(x => x.Score)
                            })
                            .FirstOrDefaultAsync();
                        report.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                        report.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                        report.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                        report.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                        report.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                    }
                    break;
                case ReportPeriod.Quarterly:
                    DateTime quarterStar = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                    DateTime quarterEnd = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1);
                    for (DateTime date = quarterStar; date <= quarterEnd; date = date.AddMonths(3))
                    {
                        var facebookMetrics = await _unitOfWork.FacebookPostMetricRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                            .GroupBy(x => 1)
                            .Select(g => new
                            {
                                TotalComments = g.Sum(x => x.CommentCount),
                                TotalImpressions = g.Sum(x => x.ImpressionCount),
                                TotalShares = g.Sum(x => x.ShareCount),
                                TotalReactions = g.Sum(x => x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount),
                                TotalScore = g.Sum(x => x.Score)
                            })
                            .FirstOrDefaultAsync();
                        report.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                        report.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                        report.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                        report.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                        report.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                        var twitterMetrics = await _unitOfWork.TwitterPostMetricRepository.Query()
                            .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                            .GroupBy(x => 1)
                            .Select(g => new
                            {
                                TotalImpressions = g.Sum(x => x.ImpressionCount),
                                TotalLikes = g.Sum(x => x.LikeCount),
                                TotalReplies = g.Sum(x => x.ReplyCount),
                                TotalRetweets = g.Sum(x => x.RetweetCount + x.QuoteCount),
                                TotalScore = g.Sum(x => x.Score)
                            })
                            .FirstOrDefaultAsync();
                        report.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                        report.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                        report.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                        report.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                        report.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                    }
                    break;
                case ReportPeriod.Yearly:
                    for (int year = startDate.Year; year <= endDate.Year; year++)
                    {
                        var facebookMetrics = await _unitOfWork.FacebookPostMetricRepository.Query()
                            .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                            .GroupBy(x => 1)
                            .Select(g => new
                            {
                                TotalComments = g.Sum(x => x.CommentCount),
                                TotalImpressions = g.Sum(x => x.ImpressionCount),
                                TotalShares = g.Sum(x => x.ShareCount),
                                TotalReactions = g.Sum(x => x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount),
                                TotalScore = g.Sum(x => x.Score)
                            })
                            .FirstOrDefaultAsync();
                        report.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                        report.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                        report.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                        report.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                        report.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                        var twitterMetrics = await _unitOfWork.TwitterPostMetricRepository.Query()
                            .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                            .GroupBy(x => 1)
                            .Select(g => new
                            {
                                TotalImpressions = g.Sum(x => x.ImpressionCount),
                                TotalLikes = g.Sum(x => x.LikeCount),
                                TotalReplies = g.Sum(x => x.ReplyCount),
                                TotalRetweets = g.Sum(x => x.RetweetCount + x.QuoteCount),
                                TotalScore = g.Sum(x => x.Score)
                            })
                            .FirstOrDefaultAsync();
                        report.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                        report.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                        report.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                        report.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                        report.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                    }
                    break;
            }
            return report;
        }

        public async Task<List<ReportSocialMediaProvinceDTO>> GetSocialMediaProvinceReport(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            var facebookReports = await _unitOfWork.FacebookPostMetricRepository
                .Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .GroupBy(x => new { x.SocialMediaPost.EntityId, x.SocialMediaPost.EntityType })
                .Select(g => new
                {
                    g.Key.EntityId,
                    g.Key.EntityType,
                    TotalScore = g.Sum(x => x.Score),
                    TotalPost = g.Count(),
                }).ToListAsync();
            var twitterReports = await _unitOfWork.TwitterPostMetricRepository
                .Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .GroupBy(x => new { x.SocialMediaPost.EntityId, x.SocialMediaPost.EntityType })
                .Select(g => new
                {
                    g.Key.EntityId,
                    g.Key.EntityType,
                    TotalScore = g.Sum(x => x.Score),
                    TotalPost = g.Count(),
                }).ToListAsync();
            var postMetrics = await _unitOfWork.PostMetricRepository
                .Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .Select(x => new
                {
                    x.PostId,
                    x.Score,
                    x.Post.ProvinceId
                }).ToListAsync();
            var attractionMetrics = await _unitOfWork.AttractionMetricRepository
                .Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .Select(x => new
                {
                    x.AttractionId,
                    x.Score,
                    x.Attraction.ProvinceId,
                }).ToListAsync();
            var tourTemplateMetrics = await _unitOfWork.TourTemplateMetricRepository
                .Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .Select(x => new
                {
                    x.TourTemplateId,
                    x.Score,
                    ProvinceIds = x.TourTemplate.TourTemplateProvinces.Select(x => x.ProvinceId).ToList(),
                }).ToListAsync();

            HashSet<string> postIds = new();
            HashSet<string> attractionIds = new();
            HashSet<string> tourTemplateIds = new();
            foreach (var report in facebookReports)
            {
                switch (report.EntityType)
                {
                    case SocialMediaPostEntity.Attraction:
                        attractionIds.Add(report.EntityId!);
                        break;
                    case SocialMediaPostEntity.Post:
                        postIds.Add(report.EntityId!);
                        break;
                    case SocialMediaPostEntity.TourTemplate:
                        tourTemplateIds.Add(report.EntityId!);
                        break;
                }
            }
            foreach (var report in twitterReports)
            {
                switch (report.EntityType)
                {
                    case SocialMediaPostEntity.Attraction:
                        attractionIds.Add(report.EntityId!);
                        break;
                    case SocialMediaPostEntity.Post:
                        postIds.Add(report.EntityId!);
                        break;
                    case SocialMediaPostEntity.TourTemplate:
                        tourTemplateIds.Add(report.EntityId!);
                        break;
                }
            }

            var postProvince = await _unitOfWork.PostRepository.Query()
                .Where(x => postIds.Contains(x.PostId!))
                .ToDictionaryAsync(x => x.PostId!, x => x.ProvinceId!);
            var attractionProvince = await _unitOfWork.AttractionRepository.Query()
                .Where(x => attractionIds.Contains(x.AttractionId!))
                .ToDictionaryAsync(x => x.AttractionId!, x => x.ProvinceId!);
            var tourTemplateProvinces = await _unitOfWork.TourTemplateRepository.Query()
                .Where(x => tourTemplateIds.Contains(x.TourTemplateId!))
                .ToDictionaryAsync(x => x.TourTemplateId!, x => x.TourTemplateProvinces.Select(x => x.ProvinceId!).ToList());

            Dictionary<string, ReportSocialMediaProvinceDTO> provinces = (await _unitOfWork.ProvinceRepository
                .Query()
                .Select(x => new { x.ProvinceId, x.Name })
                .ToListAsync())
                .ToDictionary(
                    x => x.ProvinceId!,
                    x => new ReportSocialMediaProvinceDTO
                    {
                        ProvinceId = x.ProvinceId,
                        ProvinceName = x.Name,
                        AverageAttractionScore = 0,
                        AverageFacebookScore = 0,
                        AverageSitePostScore = 0,
                        AverageScore = 0,
                        AverageTourTemplateScore = 0,
                        AverageXScore = 0,
                        TotalAttraction = 0,
                        TotalFacebookPost = 0,
                        TotalSitePost = 0,
                        TotalTourTemplate = 0,
                        TotalXPost = 0
                    }
                );

            foreach (var report in facebookReports)
            {
                List<string> provinceIds = report.EntityType switch
                {
                    SocialMediaPostEntity.Attraction => [attractionProvince[report.EntityId!]],
                    SocialMediaPostEntity.Post => [postProvince[report.EntityId!]],
                    SocialMediaPostEntity.TourTemplate => tourTemplateProvinces[report.EntityId!],
                    _ => []
                };
                foreach (string provinceId in provinceIds) 
                {
                    if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvinceDTO? value))
                    {
                        value.TotalFacebookPost += report.TotalPost;
                        value.AverageFacebookScore += report.TotalScore;
                    } 
                }
            }
            foreach (var report in twitterReports)
            {

                List<string> provinceIds = report.EntityType switch
                {
                    SocialMediaPostEntity.Attraction => [attractionProvince[report.EntityId!]],
                    SocialMediaPostEntity.Post => [postProvince[report.EntityId!]],
                    SocialMediaPostEntity.TourTemplate => tourTemplateProvinces[report.EntityId!],
                    _ => []
                };
                foreach (string provinceId in provinceIds)
                {
                    if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvinceDTO? value))
                    {
                        value.TotalXPost += report.TotalPost;
                        value.AverageXScore += report.TotalScore;
                    }
                }
            }
            foreach (var report in postMetrics)
            {
                if (provinces.TryGetValue(report.ProvinceId, out ReportSocialMediaProvinceDTO? value))
                {
                    value.TotalSitePost++;
                    value.AverageSitePostScore += report.Score;
                }
            }
            foreach (var report in attractionMetrics)
            {
                if (provinces.TryGetValue(report.ProvinceId, out ReportSocialMediaProvinceDTO? value))
                {
                    value.TotalAttraction++;
                    value.AverageAttractionScore += report.Score;
                }
            }
            foreach (var report in tourTemplateMetrics)
            {
                foreach (var provinceId in report.ProvinceIds)
                {
                    if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvinceDTO? value))
                    {
                        value.TotalTourTemplate++;
                        value.AverageTourTemplateScore += report.Score;
                    }
                }
            }
            foreach (var province in provinces.Values)
            {
                province.AverageFacebookScore = province.TotalFacebookPost == 0 ? 0 : province.AverageFacebookScore / province.TotalFacebookPost;
                province.AverageXScore = province.TotalXPost == 0 ? 0 : province.AverageXScore / province.TotalXPost;
                province.AverageSitePostScore = province.TotalSitePost == 0 ? 0 : province.AverageSitePostScore / province.TotalSitePost;
                province.AverageAttractionScore = province.TotalAttraction == 0 ? 0 : province.AverageAttractionScore / province.TotalAttraction;
                province.AverageTourTemplateScore = province.TotalTourTemplate == 0 ? 0 : province.AverageTourTemplateScore / province.TotalTourTemplate;
                province.AverageScore = (province.AverageFacebookScore + province.AverageXScore + province.AverageSitePostScore + province.AverageAttractionScore + province.AverageTourTemplateScore) / 5;
            }
            return provinces.Values.ToList();
        }


        private List<string> GetPeriodLabels(DateTime startDate, DateTime endDate)
        {
            ReportPeriod period = GetPeriod(startDate, endDate);
            List<string> periods = [];

            switch (period)
            {
                case ReportPeriod.Daily:
                    for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                    {
                        periods.Add(date.ToString("dd/MM/yyyy"));
                    }
                    break;

                case ReportPeriod.Monthly:
                    DateTime monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                    DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1); // Correct end date to last day of month
                    for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                    {
                        periods.Add(date.ToString("MM/yyyy"));
                    }
                    break;

                case ReportPeriod.Quarterly:
                    DateTime quarterStart = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                    DateTime quarterEnd = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1);
                    for (DateTime date = quarterStart; date <= quarterEnd; date = date.AddMonths(3))
                    {
                        periods.Add($"Q{(date.Month - 1) / 3 + 1}/{date.Year}");
                    }
                    break;

                case ReportPeriod.Yearly:
                    for (int year = startDate.Year; year <= endDate.Year; year++)
                    {
                        periods.Add(year.ToString());
                    }
                    break;
            }

            return periods;
        }
        private ReportPeriod GetPeriod(DateTime startDate, DateTime endDate)
        {
            TimeSpan dateDiff = (endDate - startDate);
            return dateDiff.TotalDays switch
            {
                <= 30 => ReportPeriod.Daily,
                <= 365 => ReportPeriod.Monthly,
                <= 1095 => ReportPeriod.Quarterly,
                _ => ReportPeriod.Yearly
            };
        }
        enum ReportPeriod
        {
            Daily,
            Monthly,
            Quarterly,
            Yearly
        }
    }
}
