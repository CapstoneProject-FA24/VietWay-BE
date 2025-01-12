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

namespace VietWay.Service.Management.Implement
{
    public class ReportService(IUnitOfWork unitOfWork) : IReportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        #region Revenue reports
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
        #endregion

        public async Task<ReportSocialMediaAttractionCategoryDetailDTO> GetSocialMediaAttractionCategoryDetailReport(DateTime startDate, DateTime endDate, string attractionCategoryId)
        {

            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var report = await _unitOfWork.AttractionReportRepository.Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.AttractionCategoryId.Equals(attractionCategoryId))
                .GroupBy(x => new { x.AttractionCategoryId, x.AttractionCategory.Name })
                .Select(g => new ReportSocialMediaAttractionCategoryDetailDTO
                {
                    AttractionCategoryId = g.Key.AttractionCategoryId,
                    AttractionCategoryName = g.Key.Name,
                    AverageFacebookScore = g.Average(x => x.FacebookScore),
                    AverageXScore = g.Average(x => x.XScore),
                    AverageScore = g.Average(x => x.AverageScore),
                    AverageAttractionScore = g.Average(x => x.SiteScore),
                    TotalFacebookPost = g.SelectMany(x => x.AttractionCategory.Attractions).SelectMany(x => x.SocialMediaPosts
                        .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate))).Count(),
                    TotalXPost = g.SelectMany(x => x.AttractionCategory.Attractions).SelectMany(x => x.SocialMediaPosts
                        .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate))).Count(),
                    TotalAttraction = g.SelectMany(x => x.AttractionCategory.Attractions).Where(x => x.AttractionMetrics
                        .Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count()
                })
                .FirstOrDefaultAsync() ?? throw new ResourceNotFoundException() ;
            
            var postReport = await _unitOfWork.AttractionReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.AttractionCategoryId.Equals(attractionCategoryId))
                .Select(x => new
                {
                    ReportLabel = x.ReportLabel!,
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            report.ReportSocialMediaSummary = new();
            report.ReportSocialMediaSummary.Dates = labels;
            report.ReportSocialMediaSummary.FacebookComments = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookCommentCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookShares = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookShareCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookReactions = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookReactionCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookImpressions = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookImpressionCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookScore = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookScore : 0).ToList();
            report.ReportSocialMediaSummary.XRetweets = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XRetweetCount : 0).ToList();
            report.ReportSocialMediaSummary.XReplies = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XReplyCount + p.XQuoteCount : 0).ToList();
            report.ReportSocialMediaSummary.XLikes = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XLikeCount : 0).ToList();
            report.ReportSocialMediaSummary.XImpressions = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XImpressionCount : 0).ToList();
            report.ReportSocialMediaSummary.XScore = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XScore : 0).ToList();
            var provinces = await _unitOfWork.ProvinceRepository.Query()
                .GroupBy(x => new { x.ProvinceId, x.Name })
                .Select(g => new ReportSocialMediaProvinceAttractionCategoryDTO
                {
                    ProvinceId = g.Key.ProvinceId,
                    ProvinceName = g.Key.Name,
                    TotalAttraction = g.SelectMany(x => x.Attractions).SelectMany(x => x.AttractionMetrics)
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).Count(),
                    AverageAttractionScore = g.SelectMany(x => x.AttractionReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.AverageScore),
                    AverageXScore = g.SelectMany(x => x.PostReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.XScore),
                    AverageFacebookScore = g.SelectMany(x => x.PostReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.FacebookScore),
                    TotalFacebookPost = g.SelectMany(x => x.Posts).SelectMany(x => x.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    TotalXPost = g.SelectMany(x => x.Posts).SelectMany(x => x.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                })
                .ToListAsync();
            foreach (var item in provinces)
            {
                item.AverageScore = (item.AverageFacebookScore + item.AverageXScore + item.AverageAttractionScore) / 3;
            }
            report.Provinces = provinces;
            return report;
        }

        public async Task<List<ReportSocialMediaAttractionCategoryDTO>> GetSocialMediaAttractionCategoryReport(DateTime startDate, DateTime endDate)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var result = await _unitOfWork.AttractionReportRepository.Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod)
                .GroupBy(x => new { x.AttractionCategoryId, x.AttractionCategory.Name })
                .Select(g => new ReportSocialMediaAttractionCategoryDTO
                {
                    AttractionCategoryId = g.Key.AttractionCategoryId,
                    AttractionCategoryName = g.Key.Name,
                    AverageFacebookScore = g.Sum(x => x.FacebookScore),
                    AverageXScore = g.Sum(x => x.XScore),
                    TotalFacebookPost = g.First().AttractionCategory.Attractions
                        .SelectMany(attraction => attraction.SocialMediaPosts)
                        .Count(post =>
                            post.Site == SocialMediaSite.Facebook &&
                            post.FacebookPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    TotalXPost = g.First().AttractionCategory.Attractions
                        .SelectMany(attraction => attraction.SocialMediaPosts)
                        .Count(post =>
                            post.Site == SocialMediaSite.Twitter &&
                            post.TwitterPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    AverageAttractionScore = g.Sum(x => x.SiteScore),
                    TotalAttraction = g.First().AttractionCategory.Attractions
                        .Count(x => x.AttractionMetrics
                            .Any(metric => metric.CreatedAt >= startDate &&
                                          metric.CreatedAt <= endDate))
                })
                .ToListAsync();
            foreach (var item in result)
            {
                item.AverageFacebookScore = item.TotalFacebookPost == 0 ? 0 : item.AverageFacebookScore / item.TotalFacebookPost;
                item.AverageXScore = item.TotalXPost == 0 ? 0 : item.AverageXScore / item.TotalXPost;
                item.AverageAttractionScore = item.TotalAttraction == 0 ? 0 : item.AverageAttractionScore / item.TotalAttraction;
                item.AverageScore = (item.AverageFacebookScore + item.AverageXScore + item.AverageAttractionScore) / 3;
            }
            return result;
        }

        public async Task<List<ReportSocialMediaHashtagDTO>> GetSocialMediaHashtagReport(DateTime startDate, DateTime endDate)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var result = await _unitOfWork.HashtagReportRepository.Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod)
                .GroupBy(x => new { x.HashtagId, x.Hashtag.HashtagName })
                .Select(g => new ReportSocialMediaHashtagDTO
                {
                    HashtagName = g.Key.HashtagName,
                    HashtagId = g.Key.HashtagId,
                    AverageFacebookScore = g.Sum(x => x.FacebookScore),
                    AverageXScore = g.Sum(x => x.XScore),
                    FacebookCTR = g.Sum(x => x.FacebookCTR),
                    XCTR = g.Sum(x => x.XCTR),
                    TotalFacebookPost = g.First().Hashtag.SocialMediaPostHashtags
                        .Select(x=>x.SocialMediaPost)
                        .Count(post =>
                            post.Site == SocialMediaSite.Facebook &&
                            post.FacebookPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    TotalXPost = g.First().Hashtag.SocialMediaPostHashtags
                    .Select(x => x.SocialMediaPost)
                        .Count(post =>
                            post.Site == SocialMediaSite.Twitter &&
                            post.FacebookPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        )
                })
                .ToListAsync();
            foreach (var item in result)
            {
                item.AverageFacebookScore = item.TotalFacebookPost == 0 ? 0 : item.AverageFacebookScore / item.TotalFacebookPost;
                item.AverageXScore = item.TotalXPost == 0 ? 0 : item.AverageXScore / item.TotalXPost;
                item.FacebookCTR = item.TotalFacebookPost == 0 ? 0 : item.FacebookCTR / item.TotalFacebookPost;
                item.XCTR = item.TotalXPost == 0 ? 0 : item.XCTR / item.TotalXPost;
                item.AverageScore = (item.AverageFacebookScore + item.AverageXScore) / 2;
            }
            return result;
        }

        public async Task<ReportSocialMediaHashtagDetailDTO> GetSocialMediaHashtagDetailReport(DateTime startDate, DateTime endDate, string hashtagId)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var report = await _unitOfWork.HashtagReportRepository.Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod)
                .GroupBy(x => new { x.HashtagId, x.Hashtag.HashtagName })
                .Select(g => new ReportSocialMediaHashtagDetailDTO
                {
                    HashtagName = g.Key.HashtagName,
                    HashtagId = g.Key.HashtagId,
                    AverageFacebookScore = g.Average(x => x.FacebookScore),
                    AverageXScore = g.Average(x => x.XScore),
                    FacebookCTR = g.Average(x => x.FacebookCTR),
                    XCTR = g.Average(x => x.XCTR),
                    TotalFacebookPost = g.SelectMany(x => x.Hashtag.SocialMediaPostHashtags)
                        .Where(x => x.SocialMediaPost.Site == SocialMediaSite.Facebook).Count(),
                    TotalXPost = g.SelectMany(x => x.Hashtag.SocialMediaPostHashtags)
                        .Where(x => x.SocialMediaPost.Site == SocialMediaSite.Twitter).Count()
                })
                .FirstOrDefaultAsync() ?? throw new ResourceNotFoundException();

            var hashtagReport = await _unitOfWork.HashtagReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.HashtagId.Equals(hashtagId))
                .Select(x => new
                {
                    ReportLabel = x.ReportLabel!,
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            report.ReportSocialMediaSummary = new();
            report.ReportSocialMediaSummary.Dates = labels;
            report.ReportSocialMediaSummary.FacebookComments = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.FacebookCommentCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookShares = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.FacebookShareCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookReactions = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.FacebookReactionCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookImpressions = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.FacebookImpressionCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookScore = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.FacebookScore : 0).ToList();
            report.ReportSocialMediaSummary.XRetweets = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.XRetweetCount : 0).ToList();
            report.ReportSocialMediaSummary.XReplies = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.XReplyCount + p.XQuoteCount : 0).ToList();
            report.ReportSocialMediaSummary.XLikes = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.XLikeCount : 0).ToList();
            report.ReportSocialMediaSummary.XImpressions = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.XImpressionCount : 0).ToList();
            report.ReportSocialMediaSummary.XScore = labels.Select(label => hashtagReport.TryGetValue(label, out var p) ? p.XScore : 0).ToList();

            return report;
        }

        public async Task<ReportPromotionSummaryDTO> GetPromotionSummaryAsync(DateTime startDate, DateTime endDate)
        {
            NormalizePeriod(ref startDate, ref endDate);
            var facebookPosts = await _unitOfWork.FacebookPostMetricRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalPosts = g.Select(x => x.SocialPostId).Distinct().Count(),
                    TotalComments = g.Sum(x => x.CommentCount),
                    TotalImpressions = g.Sum(x => x.ImpressionCount),
                    TotalShares = g.Sum(x => x.ShareCount),
                    TotalReactions = g.Sum(x => x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount),
                }).FirstOrDefaultAsync();
            var xPosts = await _unitOfWork.TwitterPostMetricRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .GroupBy(x => 1)
                .Select(g => new
                {
                    TotalPosts = g.Select(x => x.SocialPostId).Distinct().Count(),
                    TotalImpressions = g.Sum(x => x.ImpressionCount),
                    TotalLikes = g.Sum(x => x.LikeCount),
                    TotalReplies = g.Sum(x => x.ReplyCount),
                    TotalRetweets = g.Sum(x => x.RetweetCount + x.QuoteCount),
                })
                .FirstOrDefaultAsync();
            return new ReportPromotionSummaryDTO
            {
                TotalFacebookPost = facebookPosts?.TotalPosts??0,
                FacebookCommentCount = facebookPosts?.TotalComments??0,
                FacebookImpressionCount = facebookPosts?.TotalImpressions??0,
                FacebookShareCount = facebookPosts?.TotalShares??0,
                FacebookReactionCount = facebookPosts?.TotalReactions??0,
                TotalXPost = xPosts?.TotalPosts??0,
                XImpressionCount = xPosts?.TotalImpressions??0,
                XLikeCount = xPosts?.TotalLikes??0,
                XReplyCount = xPosts?.TotalReplies??0,
                XRetweetCount = xPosts?.TotalRetweets??0,
            };
        }

        public async Task<ReportSocialMediaPostCategoryDetailDTO> GetSocialMediaPostCategoryDetailReport(DateTime startDate, DateTime endDate, string postCategoryId)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var report = await _unitOfWork.PostReportRepository.Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.PostCategoryId.Equals(postCategoryId))
                .GroupBy(x => new { x.PostCategoryId, x.PostCategory.Name })
                .Select(g => new ReportSocialMediaPostCategoryDetailDTO
                {
                    PostCategoryId = g.Key.PostCategoryId,
                    PostCategoryName = g.Key.Name,
                    AverageFacebookScore = g.Average(x => x.FacebookScore),
                    AverageXScore = g.Sum(x => x.XScore),
                    AverageScore = g.Sum(x => x.AverageScore),
                    AverageSitePostScore = g.Sum(x => x.SiteScore),
                    TotalFacebookPost = g.First().PostCategory.Posts
                        .SelectMany(attraction => attraction.SocialMediaPosts)
                        .Count(post =>
                            post.Site == SocialMediaSite.Facebook &&
                            post.FacebookPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    TotalXPost = g.First().PostCategory.Posts
                        .SelectMany(attraction => attraction.SocialMediaPosts)
                        .Count(post =>
                            post.Site == SocialMediaSite.Twitter &&
                            post.TwitterPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    TotalSitePost = g.First().PostCategory.Posts
                        .Count(x => x.PostMetrics
                            .Any(metric => metric.CreatedAt >= startDate &&
                                          metric.CreatedAt <= endDate))
                })
                .FirstAsync();

            var postReport = await _unitOfWork.PostReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.PostCategoryId.Equals(postCategoryId))
                .Select(x => new
                {
                    ReportLabel = x.ReportLabel!,
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            report.ReportSocialMediaSummary = new();
            report.ReportSocialMediaSummary.Dates = labels;
            report.ReportSocialMediaSummary.FacebookComments = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookCommentCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookShares = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookShareCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookReactions = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookReactionCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookImpressions = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.FacebookImpressionCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookScore = labels.Select(label =>postReport.TryGetValue(label, out var p) ? p.FacebookScore : 0).ToList();
            report.ReportSocialMediaSummary.XRetweets = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XRetweetCount : 0).ToList();
            report.ReportSocialMediaSummary.XReplies = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XReplyCount + p.XQuoteCount : 0).ToList();
            report.ReportSocialMediaSummary.XLikes = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XLikeCount : 0).ToList();
            report.ReportSocialMediaSummary.XImpressions = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XImpressionCount : 0).ToList();
            report.ReportSocialMediaSummary.XScore = labels.Select(label => postReport.TryGetValue(label, out var p) ? p.XScore : 0).ToList();
            var provinces = await _unitOfWork.ProvinceRepository.Query()
                .GroupBy(x => new { x.ProvinceId, x.Name })
                .Select(g => new ReportSocialMediaProvincePostCategoryDTO
                {
                    ProvinceId = g.Key.ProvinceId,
                    ProvinceName = g.Key.Name,
                    TotalSitePost = g.SelectMany(x => x.Posts).SelectMany(x => x.PostMetrics)
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).Count(),
                    AverageSitePostScore = g.SelectMany(x => x.PostReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.AverageScore),
                    AverageXScore = g.SelectMany(x => x.PostReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.XScore),
                    AverageFacebookScore = g.SelectMany(x => x.PostReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.FacebookScore),
                    TotalFacebookPost = g.SelectMany(x => x.Posts).SelectMany(x => x.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    TotalXPost = g.SelectMany(x => x.Posts).SelectMany(x => x.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                })
                .ToListAsync();
            foreach (var item in provinces)
            {
                item.AverageScore = (item.AverageFacebookScore + item.AverageXScore  + item.AverageSitePostScore ) / 3;
            }
            report.Provinces = provinces;
            return report;
        }

        public async Task<List<ReportSocialMediaPostCategoryDTO>> GetSocialMediaPostCategoryReport(DateTime startDate, DateTime endDate)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var result = await _unitOfWork.PostReportRepository.Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod)
                .GroupBy(x => new { x.PostCategoryId, x.PostCategory.Name })
                .Select(g => new ReportSocialMediaPostCategoryDTO
                {
                    PostCategoryId = g.Key.PostCategoryId,
                    PostCategoryName = g.Key.Name,
                    AverageFacebookScore = g.Sum(x => x.FacebookScore),
                    AverageXScore = g.Sum(x => x.XScore),
                    AverageSitePostScore = g.Sum(x => x.SiteScore),
                    TotalFacebookPost = g.First().PostCategory.Posts
                        .SelectMany(attraction => attraction.SocialMediaPosts)
                        .Count(post =>
                            post.Site == SocialMediaSite.Facebook &&
                            post.FacebookPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    TotalXPost = g.First().PostCategory.Posts
                        .SelectMany(attraction => attraction.SocialMediaPosts)
                        .Count(post =>
                            post.Site == SocialMediaSite.Twitter &&
                            post.TwitterPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    TotalSitePost = g.First().PostCategory.Posts
                        .Count(x => x.PostMetrics
                            .Any(metric => metric.CreatedAt >= startDate &&
                                          metric.CreatedAt <= endDate))
                })
                .ToListAsync();
            foreach (var item in result)
            {
                item.AverageFacebookScore = item.TotalFacebookPost == 0 ? 0 : item.AverageFacebookScore / item.TotalFacebookPost;
                item.AverageXScore = item.TotalXPost == 0 ? 0 : item.AverageXScore / item.TotalXPost;
                item.AverageSitePostScore = item.TotalSitePost == 0 ? 0 : item.AverageSitePostScore / item.TotalSitePost;
                item.AverageScore = (item.AverageFacebookScore + item.AverageXScore + item.AverageSitePostScore) / 3;
            }
            return result;
        }

        public async Task<ReportSocialMediaProvinceDetailDTO> GetSocialMediaProvinceDetailReport(DateTime startDate, DateTime endDate, string provinceId)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var result = await _unitOfWork.ProvinceRepository.Query()
                .Where(x=>x.ProvinceId.Equals(provinceId))
                .GroupBy(x => new { x.ProvinceId, x.Name })
                .Select(g => new ReportSocialMediaProvinceDetailDTO
                {
                    ProvinceId = g.Key.ProvinceId,
                    ProvinceName = g.Key.Name,
                    TotalAttraction = g.SelectMany(x => x.Attractions).SelectMany(x => x.AttractionMetrics)
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).Count(),
                    TotalSitePost = g.SelectMany(x => x.Posts).SelectMany(x => x.PostMetrics)
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).Count(),
                    TotalTourTemplate = g.SelectMany(x => x.TourTemplateProvinces)
                        .Select(x => x.TourTemplate).Where(x => x.TourTemplateMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    AverageAttractionScore = g.SelectMany(x => x.AttractionReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.AverageScore),
                    AverageSitePostScore = g.SelectMany(x => x.PostReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.AverageScore),
                    AverageTourTemplateScore = g.SelectMany(x => x.TourTemplateReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.AverageScore),
                    AverageXScore = (g.SelectMany(x => x.PostReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.XScore) +
                        g.SelectMany(x => x.AttractionReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.XScore) +
                        g.SelectMany(x => x.TourTemplateReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.XScore)) / 3,
                    AverageFacebookScore = (g.SelectMany(x => x.PostReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.FacebookScore) +
                        g.SelectMany(x => x.AttractionReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.FacebookScore) +
                        g.SelectMany(x => x.TourTemplateReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.FacebookScore)) / 3,
                    TotalFacebookPost = g.SelectMany(x => x.Posts).SelectMany(x => x.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count() +
                        g.SelectMany(x => x.Attractions).SelectMany(x => x.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count() +
                        g.SelectMany(x => x.TourTemplateProvinces).SelectMany(x => x.TourTemplate.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    TotalXPost = g.SelectMany(x => x.Posts).SelectMany(x => x.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count() +
                        g.SelectMany(x => x.Attractions).SelectMany(x => x.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count() +
                        g.SelectMany(x => x.TourTemplateProvinces).SelectMany(x => x.TourTemplate.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                })
                .FirstOrDefaultAsync() ?? throw new ResourceNotFoundException();
            result.AverageScore = (result.AverageFacebookScore + result.AverageXScore + result.AverageAttractionScore + result.AverageSitePostScore + result.AverageTourTemplateScore) / 5;
            result.ReportSocialMediaSummary = new();


            var attractionReport = await _unitOfWork.AttractionReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.ProvinceId.Equals(provinceId))
                .Select(x => new
                {
                    ReportLabel = x.ReportLabel!,
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            var postReport = await _unitOfWork.PostReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.ProvinceId.Equals(provinceId))
                .Select(x => new
                {
                    ReportLabel = x.ReportLabel!,
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            var tourTemplateReport = await _unitOfWork.TourTemplateReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.ProvinceId.Equals(provinceId))
                .Select(x => new
                {
                    ReportLabel = x.ReportLabel!,
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            result.ReportSocialMediaSummary.Dates = labels;
            result.ReportSocialMediaSummary.FacebookComments = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookCommentCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookCommentCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookCommentCount : 0)
            ).ToList();

            result.ReportSocialMediaSummary.FacebookShares = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookShareCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookShareCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookShareCount : 0)
            ).ToList();

            result.ReportSocialMediaSummary.FacebookReactions = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookReactionCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookReactionCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookReactionCount : 0)
            ).ToList();

            result.ReportSocialMediaSummary.FacebookImpressions = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookImpressionCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookImpressionCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookImpressionCount : 0)
            ).ToList();

            result.ReportSocialMediaSummary.FacebookScore = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookScore : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookScore : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookScore : 0)
            ).ToList();

            result.ReportSocialMediaSummary.XRetweets = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XRetweetCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XRetweetCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XRetweetCount : 0)
            ).ToList();

            result.ReportSocialMediaSummary.XReplies = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XReplyCount + a.XQuoteCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XReplyCount + p.XQuoteCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XReplyCount + t.XQuoteCount : 0)
            ).ToList();

            result.ReportSocialMediaSummary.XLikes = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XLikeCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XLikeCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XLikeCount : 0)
            ).ToList();

            result.ReportSocialMediaSummary.XImpressions = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XImpressionCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XImpressionCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XImpressionCount : 0)
            ).ToList();

            result.ReportSocialMediaSummary.XScore = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XScore : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XScore : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XScore : 0)
            ).ToList();
            result.AttractionCategories = await _unitOfWork.AttractionCategoryRepository.Query()
                .GroupBy(x => new { x.AttractionCategoryId, x.Name })
                .Select(g => new ReportSocialMediaAttractionCategoryDTO
                {
                    AttractionCategoryId = g.Key.AttractionCategoryId,
                    AttractionCategoryName = g.Key.Name,
                    TotalAttraction = g.SelectMany(x => x.Attractions).SelectMany(x => x.AttractionMetrics)
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).Count(),
                    AverageAttractionScore = g.SelectMany(x => x.AttractionReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.AverageScore),
                    TotalFacebookPost = g.SelectMany(x => x.Attractions).SelectMany(x => x.SocialMediaPosts)
                        .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    TotalXPost = g.SelectMany(x => x.Attractions).SelectMany(x => x.SocialMediaPosts)
                        .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    AverageFacebookScore = g.SelectMany(x => x.AttractionReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.FacebookScore),
                    AverageXScore = g.SelectMany(x => x.AttractionReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.XScore),
                })
                .ToListAsync();
            return result;
        }

        public async Task<List<ReportSocialMediaProvinceDTO>> GetSocialMediaProvinceReport(DateTime startDate, DateTime endDate)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var result = await _unitOfWork.ProvinceRepository.Query()
                .Select(g => new ReportSocialMediaProvinceDTO
                {
                    ProvinceId = g.ProvinceId,
                    ProvinceName = g.Name,
                    TotalAttraction = g.Attractions.Count(x=>x.AttractionMetrics.Any(d=>d.CreatedAt >= startDate && d.CreatedAt <= endDate)),
                    TotalSitePost = g.Posts.Count(x => x.PostMetrics.Any(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)),
                    TotalTourTemplate = g.TourTemplateProvinces.Select(x=>x.TourTemplate).Count(x => x.TourTemplateMetrics.Any(d => d.CreatedAt >= startDate && d.CreatedAt <= endDate)),
                    AverageAttractionScore = g.AttractionReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.SiteScore),
                    AverageSitePostScore = g.PostReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.SiteScore),
                    AverageTourTemplateScore = g.TourTemplateReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.SiteScore),
                    AverageXScore = (g.PostReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.XScore) +
                        g.AttractionReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.XScore) +
                        g.TourTemplateReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.XScore)) / 3,
                    AverageFacebookScore = (g.PostReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.FacebookScore) +
                        g.AttractionReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.FacebookScore) +
                        g.TourTemplateReports.Where(x => labels.Contains(x.ReportLabel)).Sum(x => x.FacebookScore)) / 3,
                    TotalFacebookPost = g.Posts.SelectMany(x => x.SocialMediaPosts
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate))).Count() +
                        g.Attractions.SelectMany(x => x.SocialMediaPosts
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate))).Count() +
                        g.TourTemplateProvinces.SelectMany(x => x.TourTemplate.SocialMediaPosts
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate))).Count(),
                    TotalXPost = g.Posts.SelectMany(x => x.SocialMediaPosts
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate))).Count() +
                        g.Attractions.SelectMany(x => x.SocialMediaPosts
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate))).Count() +
                        g.TourTemplateProvinces.SelectMany(x => x.TourTemplate.SocialMediaPosts
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate))).Count(),
                })
                .ToListAsync();
            foreach (var item in result)
            {
                item.AverageAttractionScore = item.TotalAttraction == 0 ? 0 : item.AverageAttractionScore / item.TotalAttraction;
                item.AverageSitePostScore = item.TotalSitePost == 0 ? 0 : item.AverageSitePostScore / item.TotalSitePost;
                item.AverageTourTemplateScore = item.TotalTourTemplate == 0 ? 0 : item.AverageTourTemplateScore / item.TotalTourTemplate;
                item.AverageFacebookScore = item.TotalFacebookPost == 0 ? 0 : item.AverageFacebookScore / item.TotalFacebookPost;
                item.AverageXScore = item.TotalXPost == 0 ? 0 : item.AverageXScore / item.TotalXPost;
                item.AverageScore = (item.AverageFacebookScore + item.AverageXScore + item.AverageAttractionScore + item.AverageSitePostScore + item.AverageTourTemplateScore) / 5;
            }
            return result;
        }

        public async Task<ReportSocialMediaSummaryDTO> GetSocialMediaSummaryAsync(DateTime startDate, DateTime endDate)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportSocialMediaSummaryDTO report = new();
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var attractionReport = await _unitOfWork.AttractionReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod)
                .GroupBy(x=>x.ReportLabel)
                .Select(g => new
                {
                    ReportLabel = g.Key,
                    FacebookCommentCount = g.Sum(x=>x.FacebookCommentCount),
                    FacebookShareCount = g.Sum(x=>x.FacebookShareCount),
                    FacebookReactionCount = g.Sum(x=>x.FacebookReactionCount),
                    FacebookImpressionCount = g.Sum(x => x.FacebookImpressionCount),
                    FacebookScore = g.Sum(x => x.FacebookScore),
                    FacebookCTR =g.Sum(x => x.FacebookCTR),
                    XRetweetCount = g.Sum(x=> x.XRetweetCount),
                    XReplyCount = g.Sum( x => x.XReplyCount),
                    XLikeCount = g.Sum(x=> x.XLikeCount),
                    XQuoteCount = g.Sum(x=>x.XQuoteCount),
                    XImpressionCount = g.Sum(x=> x.XImpressionCount),
                    XScore = g.Sum(x=> x.XScore),
                    XCTR= g.Sum(x=> x.XCTR)
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            var postReport = await _unitOfWork.PostReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod)
                .GroupBy(x => x.ReportLabel)
                .Select(g => new
                {
                    ReportLabel = g.Key,
                    FacebookCommentCount = g.Sum(x => x.FacebookCommentCount),
                    FacebookShareCount = g.Sum(x => x.FacebookShareCount),
                    FacebookReactionCount = g.Sum(x => x.FacebookReactionCount),
                    FacebookImpressionCount = g.Sum(x => x.FacebookImpressionCount),
                    FacebookScore = g.Sum(x => x.FacebookScore),
                    FacebookCTR = g.Sum(x => x.FacebookCTR),
                    XRetweetCount = g.Sum(x => x.XRetweetCount),
                    XReplyCount = g.Sum(x => x.XReplyCount),
                    XLikeCount = g.Sum(x => x.XLikeCount),
                    XQuoteCount = g.Sum(x => x.XQuoteCount),
                    XImpressionCount = g.Sum(x => x.XImpressionCount),
                    XScore = g.Sum(x => x.XScore),
                    XCTR = g.Sum(x => x.XCTR)
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            var tourTemplateReport = await _unitOfWork.TourTemplateReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod)
                .GroupBy(x => x.ReportLabel)
                .Select(g => new
                {
                    ReportLabel = g.Key,
                    FacebookCommentCount = g.Sum(x => x.FacebookCommentCount),
                    FacebookShareCount = g.Sum(x => x.FacebookShareCount),
                    FacebookReactionCount = g.Sum(x => x.FacebookReactionCount),
                    FacebookImpressionCount = g.Sum(x => x.FacebookImpressionCount),
                    FacebookScore = g.Sum(x => x.FacebookScore),
                    FacebookCTR = g.Sum(x => x.FacebookCTR),
                    XRetweetCount = g.Sum(x => x.XRetweetCount),
                    XReplyCount = g.Sum(x => x.XReplyCount),
                    XLikeCount = g.Sum(x => x.XLikeCount),
                    XQuoteCount = g.Sum(x => x.XQuoteCount),
                    XImpressionCount = g.Sum(x => x.XImpressionCount),
                    XScore = g.Sum(x => x.XScore),
                    XCTR = g.Average(x => x.XCTR)
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            report.Dates = labels; report.FacebookComments = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookCommentCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookCommentCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookCommentCount : 0)
            ).ToList();

            report.FacebookShares = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookShareCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookShareCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookShareCount : 0)
            ).ToList();

            report.FacebookReactions = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookReactionCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookReactionCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookReactionCount : 0)
            ).ToList();

            report.FacebookImpressions = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookImpressionCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookImpressionCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookImpressionCount : 0)
            ).ToList();

            report.FacebookScore = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.FacebookScore : 0) +
                (postReport.TryGetValue(label, out var p) ? p.FacebookScore : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.FacebookScore : 0)
            ).ToList();

            report.XRetweets = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XRetweetCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XRetweetCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XRetweetCount : 0)
            ).ToList();

            report.XReplies = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XReplyCount + a.XQuoteCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XReplyCount + p.XQuoteCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XReplyCount + t.XQuoteCount : 0)
            ).ToList();

            report.XLikes = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XLikeCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XLikeCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XLikeCount : 0)
            ).ToList();

            report.XImpressions = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XImpressionCount : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XImpressionCount : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XImpressionCount : 0)
            ).ToList();

            report.XScore = labels.Select(label =>
                (attractionReport.TryGetValue(label, out var a) ? a.XScore : 0) +
                (postReport.TryGetValue(label, out var p) ? p.XScore : 0) +
                (tourTemplateReport.TryGetValue(label, out var t) ? t.XScore : 0)
            ).ToList();
            return report;
        }

        public async Task<ReportSocialMediaTourCategoryDetailDTO> GetSocialMediaTourCategoryDetailReport(DateTime startDate, DateTime endDate, string tourCategoryId)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var report = await _unitOfWork.TourTemplateReportRepository.Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.TourCategoryId.Equals(tourCategoryId))
                .GroupBy(x => new { x.TourCategoryId, x.TourCategory.Name })
                .Select(g => new ReportSocialMediaTourCategoryDetailDTO
                {
                    TourCategoryId = g.Key.TourCategoryId,
                    TourCategoryName = g.Key.Name,
                    AverageFacebookScore = g.Average(x => x.FacebookScore),
                    AverageXScore = g.Average(x => x.XScore),
                    AverageScore = g.Average(x => x.AverageScore),
                    TotalFacebookPost = g.SelectMany(x => x.TourCategory.TourTemplates).SelectMany(x => x.SocialMediaPosts)
                        .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    TotalXPost = g.SelectMany(x => x.TourCategory.TourTemplates).SelectMany(x => x.SocialMediaPosts)
                        .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    AverageTourTemplateScore = g.Average(x => x.AverageScore),
                    TotalTourTemplate = g.SelectMany(x => x.TourCategory.TourTemplates)
                        .Where(x => x.TourTemplateMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count()
                })
                .FirstAsync();
            var tourCategoryReport = await _unitOfWork.TourTemplateReportRepository
                .Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod && x.TourCategoryId.Equals(tourCategoryId))
                .Select(x => new
                {
                    ReportLabel = x.ReportLabel!,
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                }).ToDictionaryAsync(x => x.ReportLabel, x => new
                {
                    x.FacebookCommentCount,
                    x.FacebookShareCount,
                    x.FacebookReactionCount,
                    x.FacebookImpressionCount,
                    x.FacebookScore,
                    x.FacebookCTR,
                    x.XRetweetCount,
                    x.XReplyCount,
                    x.XLikeCount,
                    x.XQuoteCount,
                    x.XImpressionCount,
                    x.XScore,
                    x.XCTR
                });
            report.ReportSocialMediaSummary = new();
            report.ReportSocialMediaSummary.Dates = labels;
            report.ReportSocialMediaSummary.FacebookComments = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.FacebookCommentCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookShares = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.FacebookShareCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookReactions = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.FacebookReactionCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookImpressions = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.FacebookImpressionCount : 0).ToList();
            report.ReportSocialMediaSummary.FacebookScore = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.FacebookScore : 0).ToList();
            report.ReportSocialMediaSummary.XRetweets = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.XRetweetCount : 0).ToList();
            report.ReportSocialMediaSummary.XReplies = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.XReplyCount + p.XQuoteCount : 0).ToList();
            report.ReportSocialMediaSummary.XLikes = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.XLikeCount : 0).ToList();
            report.ReportSocialMediaSummary.XImpressions = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.XImpressionCount : 0).ToList();
            report.ReportSocialMediaSummary.XScore = labels.Select(label => tourCategoryReport.TryGetValue(label, out var p) ? p.XScore : 0).ToList();
            var provinces = await _unitOfWork.ProvinceRepository.Query()
                .GroupBy(x => new { x.ProvinceId, x.Name })
                .Select(g => new ReportSocialMediaProvinceTourCategoryDTO
                {
                    ProvinceId = g.Key.ProvinceId,
                    ProvinceName = g.Key.Name,
                    AverageXScore = g.SelectMany(x => x.TourTemplateReports).Where(x=>labels.Contains(x.ReportLabel)).Average(x=>x.XScore),
                    AverageFacebookScore = g.SelectMany(x => x.TourTemplateReports).Where(x => labels.Contains(x.ReportLabel)).Average(x => x.FacebookScore),
                    TotalFacebookPost = g.SelectMany(x => x.TourTemplateProvinces).SelectMany(x => x.TourTemplate.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Facebook && x.FacebookPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    TotalXPost = g.SelectMany(x => x.TourTemplateProvinces).SelectMany(x => x.TourTemplate.SocialMediaPosts)
                            .Where(x => x.Site == SocialMediaSite.Twitter && x.TwitterPostMetrics.Any(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)).Count(),
                    AverageScore = 0,
                    AverageTourTemplateScore = g.SelectMany(x => x.TourTemplateReports.Where(x => labels.Contains(x.ReportLabel))).Average(x => x.AverageScore),
                    TotalTourTemplate = g.SelectMany(x => x.TourTemplateProvinces).SelectMany(x => x.TourTemplate.TourTemplateMetrics)
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).Count(),
                })
                .ToListAsync();
            foreach (var item in provinces)
            {
                item.AverageScore = (item.AverageFacebookScore + item.AverageXScore + item.AverageTourTemplateScore) / 3;
            }
            report.Provinces = provinces;
            return report;
        }

        public async Task<List<ReportSocialMediaTourCategoryDTO>> GetSocialMediaTourTemplateCategoryReport(DateTime startDate, DateTime endDate)
        {
            NormalizePeriod(ref startDate, ref endDate);
            List<string> labels = GetPeriodLabels(startDate, endDate);
            ReportPeriod reportPeriod = GetPeriod(startDate, endDate);
            var result = await _unitOfWork.TourTemplateReportRepository.Query()
                .Where(x => labels.Contains(x.ReportLabel) && x.ReportPeriod == reportPeriod)
                .GroupBy(x => new { x.TourCategoryId, x.TourCategory.Name })
                .Select(g => new ReportSocialMediaTourCategoryDTO
                {
                    TourCategoryId = g.Key.TourCategoryId,
                    TourCategoryName = g.Key.Name,
                    AverageFacebookScore = g.Sum(x => x.FacebookScore),
                    AverageXScore = g.Sum(x => x.XScore),
                    TotalFacebookPost = g.First().TourCategory.TourTemplates
                        .SelectMany(tour => tour.SocialMediaPosts)
                        .Count(post =>
                            post.Site == SocialMediaSite.Facebook &&
                            post.FacebookPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    TotalXPost = g.First().TourCategory.TourTemplates
                        .SelectMany(t => t.SocialMediaPosts)
                        .Count(post =>
                            post.Site == SocialMediaSite.Twitter &&
                            post.TwitterPostMetrics.Any(metric =>
                                metric.CreatedAt >= startDate &&
                                metric.CreatedAt <= endDate)
                        ),
                    TotalTourTemplate = g.First().TourCategory.TourTemplates
                        .Count(x => x.TourTemplateMetrics
                            .Any(metric => metric.CreatedAt >= startDate &&
                                          metric.CreatedAt <= endDate)),
                    AverageTourTemplateScore = g.Sum(x => x.SiteScore),
                })
                .ToListAsync();
            foreach (var item in result)
            {
                item.AverageFacebookScore = item.TotalFacebookPost == 0 ? 0 : item.AverageFacebookScore / item.TotalFacebookPost;
                item.AverageXScore = item.TotalXPost == 0 ? 0 : item.AverageXScore / item.TotalXPost;
                item.AverageTourTemplateScore = item.TotalTourTemplate == 0 ? 0 : item.AverageTourTemplateScore / item.TotalTourTemplate;
                item.AverageScore = (item.AverageFacebookScore + item.AverageXScore + item.AverageTourTemplateScore) / 3;
            }
            return result;
        }

        #region Helper methods
        List<string> GetPeriodLabels(DateTime startDate, DateTime endDate)
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
                    DateTime monthStart = new(startDate.Year, startDate.Month, 1);
                    DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
                    for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                    {
                        periods.Add(date.ToString("MM/yyyy"));
                    }
                    break;

                case ReportPeriod.Quarterly:
                    DateTime quarterStart = new(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
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
        private static ReportPeriod GetPeriod(DateTime startDate, DateTime endDate)
        {
            TimeSpan dateDiff = (endDate - startDate);
            return dateDiff.TotalDays switch
            {
                <= 45 => ReportPeriod.Daily,
                //<= 365 => ReportPeriod.Monthly,
                //<= 1095 => ReportPeriod.Quarterly,
                //_ => ReportPeriod.Yearly
                _ => ReportPeriod.Monthly
            };
        }
        private static void NormalizePeriod(ref DateTime startDate, ref DateTime endDate)
        {
            ReportPeriod period = GetPeriod(startDate, endDate);
            switch (period)
            {
                case ReportPeriod.Daily:
                    startDate = startDate.Date;
                    endDate = endDate.Date.AddDays(1).AddSeconds(-1);
                    break;
                case ReportPeriod.Monthly:
                    startDate = new DateTime(startDate.Year, startDate.Month, 1);
                    endDate = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
                    break;
                case ReportPeriod.Quarterly:
                    startDate = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                    endDate = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1).AddMonths(3).AddDays(-1);
                    break;
                case ReportPeriod.Yearly:
                    startDate = new DateTime(startDate.Year, 1, 1);
                    endDate = new DateTime(endDate.Year, 12, 31).AddDays(1).AddSeconds(-1);
                    break;
            }
        }
        #endregion
    }
}