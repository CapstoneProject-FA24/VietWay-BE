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

namespace VietWay.Service.Management.Implement
{
    public class ReportService(IUnitOfWork unitOfWork) : IReportService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public Task<ReportPromotionSummaryDTO> GetPromotionSummaryAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ReportBookingDTO> GetReportBookingAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ReportRatingDTO> GetReportRatingAsync(DateTime startDate, DateTime endDate, bool isAsc)
        {
            throw new NotImplementedException();
        }

        public Task<ReportRevenueDTO> GetReportRevenueAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ReportSummaryDTO> GetReportSummaryAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ReportSocialMediaAttractionCategoryDetailDTO> GetSocialMediaAttractionCategoryDetailReport(DateTime startDate, DateTime endDate, string attractionCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReportSocialMediaAttractionCategoryDTO>> GetSocialMediaAttractionCategoryReport(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ReportSocialMediaHashtagDTO>> GetSocialMediaHashtagReport(DateTime startDate, DateTime endDate)
        {
            return await _unitOfWork.HashtagRepository.Query()
                .Where(x => true)
                .Select(x => new ReportSocialMediaHashtagDTO
                {
                    HashtagName = x.HashtagName,
                    AverageFacebookScore = 1,
                    AverageScore = 1,
                    AverageXScore = 1,
                    FacebookCTR = 1,
                    HashtagId = x.HashtagId,
                    XCTR = 1,
                    TotalFacebookPost = 1,
                    TotalXPost = 1
                })
                .ToListAsync();
        }

        public Task<ReportSocialMediaPostCategoryDetailDTO> GetSocialMediaPostCategoryDetailReport(DateTime startDate, DateTime endDate, string postCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReportSocialMediaPostCategoryDTO>> GetSocialMediaPostCategoryReport(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ReportSocialMediaProvinceDetailDTO> GetSocialMediaProvinceDetailReport(DateTime startDate, DateTime endDate, string provinceId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReportSocialMediaProvinceDTO>> GetSocialMediaProvinceReport(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ReportSocialMediaSummaryDTO> GetSocialMediaSummaryAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ReportSocialMediaTourCategoryDetailDTO> GetSocialMediaTourCategoryDetailReport(DateTime startDate, DateTime endDate, string tourCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ReportSocialMediaTourCategoryDTO>> GetSocialMediaTourTemplateCategoryReport(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        /*
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

                #region Social media reports
                public async Task<ReportPromotionSummaryDTO> GetPromotionSummaryAsync(DateTime startDate, DateTime endDate)
                {
                    NormalizePeriod(ref startDate, ref endDate);
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
                        XRetweetCount = xPosts.TotalRetweets ?? 0,
                    };
                }

                public async Task<ReportSocialMediaSummaryDTO> GetSocialMediaSummaryAsync(DateTime startDate, DateTime endDate)
                {
                    NormalizePeriod(ref startDate, ref endDate);
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
                    NormalizePeriod(ref startDate, ref endDate);

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
                        .Select(x => new { x.TourTemplateId, TourTemplateProvinces = x.TourTemplateProvinces.Select(x => x.ProvinceId).ToList() })
                        .ToDictionaryAsync(x => x.TourTemplateId!, x => x.TourTemplateProvinces);

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

                public async Task<List<ReportSocialMediaPostCategoryDTO>> GetSocialMediaPostCategoryReport(DateTime startDate, DateTime endDate)
                {
                    NormalizePeriod(ref startDate, ref endDate);
                    var facebookReports = await _unitOfWork.FacebookPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.SocialMediaPost.EntityType == SocialMediaPostEntity.Post)
                        .GroupBy(x => x.SocialMediaPost.EntityId)
                        .Select(g => new
                        {
                            EntityId = g.Key,
                            TotalScore = g.Sum(x => x.Score),
                            TotalPost = g.Count(),
                        }).ToListAsync();
                    var twitterReports = await _unitOfWork.TwitterPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.SocialMediaPost.EntityType == SocialMediaPostEntity.Post)
                        .GroupBy(x => x.SocialMediaPost.EntityId)
                        .Select(g => new
                        {
                            EntityId = g.Key,
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
                            x.Post.PostCategoryId
                        }).ToListAsync();

                    HashSet<string> postIds = new();
                    foreach (var report in facebookReports)
                    {
                        postIds.Add(report.EntityId!);
                    }
                    foreach (var report in twitterReports)
                    {
                        postIds.Add(report.EntityId!);
                    }

                    var postCategory = await _unitOfWork.PostRepository.Query()
                        .Where(x => postIds.Contains(x.PostId!))
                        .ToDictionaryAsync(x => x.PostId!, x => x.PostCategoryId!);

                    Dictionary<string, ReportSocialMediaPostCategoryDTO> postCategories = (await _unitOfWork.PostCategoryRepository
                        .Query()
                        .Select(x => new { x.PostCategoryId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.PostCategoryId!,
                            x => new ReportSocialMediaPostCategoryDTO
                            {
                                PostCategoryId = x.PostCategoryId,
                                PostCategoryName = x.Name,
                                AverageFacebookScore = 0,
                                AverageSitePostScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalSitePost = 0,
                                TotalXPost = 0
                            }
                        );

                    foreach (var report in facebookReports)
                    {
                        if (postCategories.TryGetValue(postCategory[report.EntityId], out ReportSocialMediaPostCategoryDTO? value))
                        {
                            value.TotalFacebookPost += report.TotalPost;
                            value.AverageFacebookScore += report.TotalScore;
                        }
                    }
                    foreach (var report in twitterReports)
                    {
                        if (postCategories.TryGetValue(postCategory[report.EntityId], out ReportSocialMediaPostCategoryDTO? value))
                        {
                            value.TotalXPost += report.TotalPost;
                            value.AverageXScore += report.TotalScore;
                        }
                    }
                    foreach (var report in postMetrics)
                    {
                        if (postCategories.TryGetValue(report.PostCategoryId, out ReportSocialMediaPostCategoryDTO? value))
                        {
                            value.TotalSitePost++;
                            value.AverageSitePostScore += report.Score;
                        }
                    }
                    foreach (var category in postCategories.Values)
                    {
                        category.AverageFacebookScore = category.TotalFacebookPost == 0 ? 0 : category.AverageFacebookScore / category.TotalFacebookPost;
                        category.AverageXScore = category.TotalXPost == 0 ? 0 : category.AverageXScore / category.TotalXPost;
                        category.AverageSitePostScore = category.TotalSitePost == 0 ? 0 : category.AverageSitePostScore / category.TotalSitePost;
                        category.AverageScore = (category.AverageFacebookScore + category.AverageXScore + category.AverageSitePostScore) / 3;
                    }
                    return postCategories.Values.ToList();
                }

                public async Task<List<ReportSocialMediaAttractionCategoryDTO>> GetSocialMediaAttractionCategoryReport(DateTime startDate, DateTime endDate)
                {
                    NormalizePeriod(ref startDate, ref endDate);

                    var facebookReports = await _unitOfWork.FacebookPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.SocialMediaPost.EntityType == SocialMediaPostEntity.Attraction)
                        .GroupBy(x => x.SocialMediaPost.EntityId)
                        .Select(g => new
                        {
                            EntityId = g.Key,
                            TotalScore = g.Sum(x => x.Score),
                            TotalPost = g.Count(),
                        }).ToListAsync();
                    var twitterReports = await _unitOfWork.TwitterPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.SocialMediaPost.EntityType == SocialMediaPostEntity.Attraction)
                        .GroupBy(x => x.SocialMediaPost.EntityId)
                        .Select(g => new
                        {
                            EntityId = g.Key,
                            TotalScore = g.Sum(x => x.Score),
                            TotalPost = g.Count(),
                        }).ToListAsync();
                    var attractionMetrics = await _unitOfWork.AttractionMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                        .Select(x => new
                        {
                            x.AttractionId,
                            x.Score,
                            x.Attraction.AttractionCategoryId
                        }).ToListAsync();

                    HashSet<string> attractionIds = new();
                    foreach (var report in facebookReports)
                    {
                        attractionIds.Add(report.EntityId!);
                    }
                    foreach (var report in twitterReports)
                    {
                        attractionIds.Add(report.EntityId!);
                    }

                    var attractionCategory = await _unitOfWork.AttractionRepository.Query()
                        .Where(x => attractionIds.Contains(x.AttractionId!))
                        .ToDictionaryAsync(x => x.AttractionId!, x => x.AttractionCategoryId!);

                    Dictionary<string, ReportSocialMediaAttractionCategoryDTO> attractionCategories = (await _unitOfWork.AttractionCategoryRepository
                        .Query()
                        .Select(x => new { x.AttractionCategoryId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.AttractionCategoryId!,
                            x => new ReportSocialMediaAttractionCategoryDTO
                            {
                                AttractionCategoryId = x.AttractionCategoryId,
                                AttractionCategoryName = x.Name,
                                AverageFacebookScore = 0,
                                AverageAttractionScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalAttraction = 0,
                                TotalXPost = 0
                            }
                        );

                    foreach (var report in facebookReports)
                    {
                        if (attractionCategories.TryGetValue(attractionCategory[report.EntityId], out ReportSocialMediaAttractionCategoryDTO? value))
                        {
                            value.TotalFacebookPost += report.TotalPost;
                            value.AverageFacebookScore += report.TotalScore;
                        }
                    }
                    foreach (var report in twitterReports)
                    {
                        if (attractionCategories.TryGetValue(attractionCategory[report.EntityId], out ReportSocialMediaAttractionCategoryDTO? value))
                        {
                            value.TotalXPost += report.TotalPost;
                            value.AverageXScore += report.TotalScore;
                        }
                    }
                    foreach (var report in attractionMetrics)
                    {
                        if (attractionCategories.TryGetValue(report.AttractionCategoryId, out ReportSocialMediaAttractionCategoryDTO? value))
                        {
                            value.TotalAttraction++;
                            value.AverageAttractionScore += report.Score;
                        }
                    }
                    foreach (var category in attractionCategories.Values)
                    {
                        category.AverageFacebookScore = category.TotalFacebookPost == 0 ? 0 : category.AverageFacebookScore / category.TotalFacebookPost;
                        category.AverageXScore = category.TotalXPost == 0 ? 0 : category.AverageXScore / category.TotalXPost;
                        category.AverageAttractionScore = category.TotalAttraction == 0 ? 0 : category.AverageAttractionScore / category.TotalAttraction;
                        category.AverageScore = (category.AverageFacebookScore + category.AverageXScore + category.AverageAttractionScore) / 3;
                    }
                    return attractionCategories.Values.ToList();
                }

                public async Task<List<ReportSocialMediaTourCategoryDTO>> GetSocialMediaTourTemplateCategoryReport(DateTime startDate, DateTime endDate)
                {
                    NormalizePeriod(ref startDate, ref endDate);

                    var facebookReports = await _unitOfWork.FacebookPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.SocialMediaPost.EntityType == SocialMediaPostEntity.TourTemplate)
                        .GroupBy(x => x.SocialMediaPost.EntityId)
                        .Select(g => new
                        {
                            EntityId = g.Key,
                            TotalScore = g.Sum(x => x.Score),
                            TotalPost = g.Count(),
                        }).ToListAsync();
                    var twitterReports = await _unitOfWork.TwitterPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.SocialMediaPost.EntityType == SocialMediaPostEntity.TourTemplate)
                        .GroupBy(x => x.SocialMediaPost.EntityId)
                        .Select(g => new
                        {
                            EntityId = g.Key,
                            TotalScore = g.Sum(x => x.Score),
                            TotalPost = g.Count(),
                        }).ToListAsync();
                    var tourTemplateMetric = await _unitOfWork.TourTemplateMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                        .Select(x => new
                        {
                            x.TourTemplateId,
                            x.Score,
                            x.TourTemplate.TourCategoryId
                        }).ToListAsync();

                    HashSet<string> tourTemplateIds = new();
                    foreach (var report in facebookReports)
                    {
                        tourTemplateIds.Add(report.EntityId!);
                    }
                    foreach (var report in twitterReports)
                    {
                        tourTemplateIds.Add(report.EntityId!);
                    }

                    var attractionCategory = await _unitOfWork.TourTemplateRepository.Query()
                        .Where(x => tourTemplateIds.Contains(x.TourTemplateId!))
                        .ToDictionaryAsync(x => x.TourTemplateId!, x => x.TourCategoryId!);

                    Dictionary<string, ReportSocialMediaTourCategoryDTO> attractionCategories = (await _unitOfWork.TourCategoryRepository
                        .Query()
                        .Select(x => new { x.TourCategoryId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.TourCategoryId!,
                            x => new ReportSocialMediaTourCategoryDTO
                            {
                                TourCategoryId = x.TourCategoryId,
                                TourCategoryName = x.Name,
                                AverageFacebookScore = 0,
                                AverageTourTemplateScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalTourTemplate = 0,
                                TotalXPost = 0
                            }
                        );

                    foreach (var report in facebookReports)
                    {
                        if (attractionCategories.TryGetValue(attractionCategory[report.EntityId], out ReportSocialMediaTourCategoryDTO? value))
                        {
                            value.TotalFacebookPost += report.TotalPost;
                            value.AverageFacebookScore += report.TotalScore;
                        }
                    }
                    foreach (var report in twitterReports)
                    {
                        if (attractionCategories.TryGetValue(attractionCategory[report.EntityId], out ReportSocialMediaTourCategoryDTO? value))
                        {
                            value.TotalXPost += report.TotalPost;
                            value.AverageXScore += report.TotalScore;
                        }
                    }
                    foreach (var report in tourTemplateMetric)
                    {
                        if (attractionCategories.TryGetValue(report.TourCategoryId, out ReportSocialMediaTourCategoryDTO? value))
                        {
                            value.TotalTourTemplate++;
                            value.AverageTourTemplateScore += report.Score;
                        }
                    }
                    foreach (var category in attractionCategories.Values)
                    {
                        category.AverageFacebookScore = category.TotalFacebookPost == 0 ? 0 : category.AverageFacebookScore / category.TotalFacebookPost;
                        category.AverageXScore = category.TotalXPost == 0 ? 0 : category.AverageXScore / category.TotalXPost;
                        category.AverageTourTemplateScore = category.TotalTourTemplate == 0 ? 0 : category.AverageTourTemplateScore / category.TotalTourTemplate;
                        category.AverageScore = (category.AverageFacebookScore + category.AverageXScore + category.AverageTourTemplateScore) / 3;
                    }
                    return attractionCategories.Values.ToList();
                }
                #endregion

                #region Province media report detail
                public async Task<ReportSocialMediaProvinceDetailDTO> GetSocialMediaProvinceDetailReport(DateTime startDate, DateTime endDate, string provinceId)
                {
                    NormalizePeriod(ref startDate, ref endDate);
                    ReportSocialMediaProvinceDetailDTO report = await _unitOfWork.ProvinceRepository.Query()
                        .Where(x => x.ProvinceId == provinceId)
                        .Select(x => new ReportSocialMediaProvinceDetailDTO
                        {
                            ProvinceId = x.ProvinceId,
                            ProvinceName = x.Name,
                        })
                        .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("NOT_EXIST_PROVINCE");

                    report.AttractionCategories = new();
                    report.PostCategories = new();
                    report.TourTemplateCategories = new();

                    var allEntityIds = await _unitOfWork.SocialMediaPostRepository
                        .Query()
                        .Where(x => x.FacebookPostMetrics.Any(f => f.CreatedAt >= startDate && f.CreatedAt <= endDate) ||
                                    x.TwitterPostMetrics.Any(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate))
                        .Select(x => new
                        {
                            x.EntityId,
                            x.EntityType,
                        })
                        .Distinct()
                        .ToListAsync();
                    List<string> postIds = allEntityIds.Where(e => e.EntityType == SocialMediaPostEntity.Post).Select(x => x.EntityId).ToList();
                    List<string> attractionIds = allEntityIds.Where(e => e.EntityType == SocialMediaPostEntity.Attraction).Select(x => x.EntityId).ToList();
                    List<string> tourTemplateIds = allEntityIds.Where(e => e.EntityType == SocialMediaPostEntity.TourTemplate).Select(x => x.EntityId).ToList();
                    var posts = await _unitOfWork.PostRepository
                        .Query()
                        .Where(x => postIds.Contains(x.PostId) && x.ProvinceId == provinceId)
                        .Select(x => new
                        {
                            x.PostId,
                            x.PostCategoryId,
                        })
                        .Distinct()
                        .ToDictionaryAsync(x => x.PostId!, x => x.PostCategoryId!);
                    var attractions = await _unitOfWork.AttractionRepository
                        .Query()
                        .Where(x => attractionIds.Contains(x.AttractionId) && x.ProvinceId == provinceId)
                        .Select(x => new
                        {
                            x.AttractionId,
                            x.AttractionCategoryId,
                        })
                        .Distinct()
                        .ToDictionaryAsync(x => x.AttractionId!, x => x.AttractionCategoryId);
                    var tourTemplates = await _unitOfWork.TourTemplateRepository
                        .Query()
                        .Where(x => tourTemplateIds.Contains(x.TourTemplateId) && x.TourTemplateProvinces.Any(x => x.ProvinceId == provinceId))
                        .Select(x => new
                        {
                            x.TourTemplateId,
                            x.TourCategoryId,
                        })
                        .Distinct()
                        .ToDictionaryAsync(x => x.TourTemplateId, x => x.TourCategoryId);

                    var provinceEntityIds = posts.Keys.Concat(attractions.Keys).Concat(tourTemplates.Keys).ToList();

                    var facebookReports = await _unitOfWork.FacebookPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && provinceEntityIds.Contains(x.SocialMediaPost.EntityId))
                        .Select(x => new
                        {
                            x.SocialPostId,
                            x.SocialMediaPost.EntityId,
                            x.SocialMediaPost.EntityType,
                            x.Score,
                            x.CommentCount,
                            x.ImpressionCount,
                            x.ShareCount,
                            x.CreatedAt,
                            ReactionCount = x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount,
                        }).ToListAsync();
                    var twitterReports = await _unitOfWork.TwitterPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && provinceEntityIds.Contains(x.SocialMediaPost.EntityId))
                        .Select(x => new
                        {
                            x.SocialPostId,
                            x.SocialMediaPost.EntityId,
                            x.SocialMediaPost.EntityType,
                            x.Score,
                            x.ImpressionCount,
                            x.LikeCount,
                            x.ReplyCount,
                            x.CreatedAt,
                            RetweetCount = x.QuoteCount + x.RetweetCount,
                        }).ToListAsync();
                    var postMetrics = await _unitOfWork.PostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.Post.ProvinceId == provinceId)
                        .Select(x => new
                        {
                            x.PostId,
                            x.Score,
                            x.Post.PostCategoryId
                        }).ToListAsync();
                    var attractionMetrics = await _unitOfWork.AttractionMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.Attraction.ProvinceId == provinceId)
                        .Select(x => new
                        {
                            x.AttractionId,
                            x.Score,
                            x.Attraction.AttractionCategoryId,
                        }).ToListAsync();
                    var tourTemplateMetrics = await _unitOfWork.TourTemplateMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.TourTemplate.TourTemplateProvinces.Any(t => t.ProvinceId == provinceId))
                        .Select(x => new
                        {
                            x.TourTemplateId,
                            x.Score,
                            x.TourTemplate.TourCategoryId
                        }).ToListAsync();
                    Dictionary<string, ReportSocialMediaPostCategoryDTO> postCategories = (await _unitOfWork.PostCategoryRepository
                        .Query()
                        .Select(x => new { x.PostCategoryId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.PostCategoryId!,
                            x => new ReportSocialMediaPostCategoryDTO
                            {
                                PostCategoryId = x.PostCategoryId,
                                PostCategoryName = x.Name,
                                AverageFacebookScore = 0,
                                AverageSitePostScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalSitePost = 0,
                                TotalXPost = 0
                            }
                        );
                    Dictionary<string, ReportSocialMediaAttractionCategoryDTO> attractionCategories = (await _unitOfWork.AttractionCategoryRepository
                        .Query()
                        .Select(x => new { x.AttractionCategoryId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.AttractionCategoryId!,
                            x => new ReportSocialMediaAttractionCategoryDTO
                            {
                                AttractionCategoryId = x.AttractionCategoryId,
                                AttractionCategoryName = x.Name,
                                AverageFacebookScore = 0,
                                AverageAttractionScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalAttraction = 0,
                                TotalXPost = 0
                            }
                        );
                    Dictionary<string, ReportSocialMediaTourCategoryDTO> tourCategories = (await _unitOfWork.TourCategoryRepository
                        .Query()
                        .Select(x => new { x.TourCategoryId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.TourCategoryId!,
                            x => new ReportSocialMediaTourCategoryDTO
                            {
                                TourCategoryId = x.TourCategoryId,
                                TourCategoryName = x.Name,
                                AverageFacebookScore = 0,
                                AverageTourTemplateScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalTourTemplate = 0,
                                TotalXPost = 0
                            }
                        );


                    report.TotalXPost = twitterReports.DistinctBy(x => x.SocialPostId).Count();
                    report.TotalTourTemplate = tourTemplateMetrics.DistinctBy(x => x.TourTemplateId).Count();
                    report.TotalSitePost = postMetrics.DistinctBy(x => x.PostId).Count();
                    report.TotalFacebookPost = facebookReports.DistinctBy(x => x.SocialPostId).Count();
                    report.TotalAttraction = attractionMetrics.DistinctBy(x => x.AttractionId).Count();
                    report.AverageXScore = twitterReports.GroupBy(x => x.SocialPostId).Select(g => g.Sum(x => x.Score)).Average();
                    report.AverageTourTemplateScore = tourTemplateMetrics.GroupBy(x => x.TourTemplateId).Select(g => g.Sum(x => x.Score)).Average();
                    report.AverageSitePostScore = postMetrics.GroupBy(x => x.PostId).Select(g => g.Sum(x => x.Score)).Average();
                    report.AverageAttractionScore = attractionMetrics.GroupBy(x => x.AttractionId).Select(g => g.Sum(x => x.Score)).Average();
                    report.AverageFacebookScore = facebookReports.GroupBy(x => x.SocialPostId).Select(g => g.Sum(x => x.Score)).Average();
                    report.AverageScore = (report.AverageXScore + report.AverageTourTemplateScore + report.AverageSitePostScore + report.AverageAttractionScore + report.AverageFacebookScore) / 5;

                    List<string> periodLabels = GetPeriodLabels(startDate, endDate);

                    report.ReportSocialMediaSummary = new ReportSocialMediaSummaryDTO
                    {
                        Dates = periodLabels,
                        FacebookComments = [],
                        FacebookImpressions = [],
                        FacebookReactions = [],
                        FacebookScore = [],
                        FacebookShares = [],
                        XImpressions = [],
                        XLikes = [],
                        XReplies = [],
                        XRetweets = [],
                        XScore = [],
                    };

                    switch (GetPeriod(startDate, endDate))
                    {
                        case ReportPeriod.Daily:
                            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Monthly:
                            DateTime monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                            DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
                            for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Quarterly:
                            DateTime quarterStar = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                            DateTime quarterEnd = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1);
                            for (DateTime date = quarterStar; date <= quarterEnd; date = date.AddMonths(3))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Yearly:
                            for (int year = startDate.Year; year <= endDate.Year; year++)
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                    }

                    foreach (var facebookReport in facebookReports.GroupBy(x => new { x.SocialPostId }).Select(g => new { EntityType = g.Select(x => x.EntityType).Distinct().Single(), EntityId = g.Select(x => x.EntityId).Distinct().Single(), Score = g.Sum(x => x.Score) }).ToList())
                    {
                        switch (facebookReport.EntityType)
                        {
                            case SocialMediaPostEntity.Attraction:
                                if (attractionCategories.TryGetValue(attractions[facebookReport.EntityId], out ReportSocialMediaAttractionCategoryDTO attractionCategoryDTO))
                                {
                                    attractionCategoryDTO.TotalFacebookPost++;
                                    attractionCategoryDTO.AverageFacebookScore += facebookReport.Score;
                                }
                                break;
                            case SocialMediaPostEntity.Post:
                                if (postCategories.TryGetValue(posts[facebookReport.EntityId], out ReportSocialMediaPostCategoryDTO postCategoryDTO))
                                {
                                    postCategoryDTO.TotalFacebookPost++;
                                    postCategoryDTO.AverageFacebookScore += facebookReport.Score;
                                }
                                break;
                            case SocialMediaPostEntity.TourTemplate:
                                if (tourCategories.TryGetValue(tourTemplates[facebookReport.EntityId], out ReportSocialMediaTourCategoryDTO tourCategoryDTO))
                                {
                                    tourCategoryDTO.TotalFacebookPost++;
                                    tourCategoryDTO.AverageFacebookScore += facebookReport.Score;
                                }
                                break;
                        }
                    }
                    foreach (var twitterReport in twitterReports.GroupBy(x => new { x.SocialPostId }).Select(g => new { EntityType = g.Select(x => x.EntityType).Distinct().Single(), EntityId = g.Select(x => x.EntityId).Distinct().Single(), Score = g.Sum(x => x.Score) }).ToList())
                    {
                        switch (twitterReport.EntityType)
                        {
                            case SocialMediaPostEntity.Attraction:
                                if (attractionCategories.TryGetValue(attractions[twitterReport.EntityId], out ReportSocialMediaAttractionCategoryDTO attractionCategoryDTO))
                                {
                                    attractionCategoryDTO.TotalXPost++;
                                    attractionCategoryDTO.AverageXScore += twitterReport.Score;
                                }
                                break;
                            case SocialMediaPostEntity.Post:
                                if (postCategories.TryGetValue(posts[twitterReport.EntityId], out ReportSocialMediaPostCategoryDTO postCategoryDTO))
                                {
                                    postCategoryDTO.TotalXPost++;
                                    postCategoryDTO.AverageXScore += twitterReport.Score;
                                }
                                break;
                            case SocialMediaPostEntity.TourTemplate:
                                if (tourCategories.TryGetValue(tourTemplates[twitterReport.EntityId], out ReportSocialMediaTourCategoryDTO tourCategoryDTO))
                                {
                                    tourCategoryDTO.TotalXPost++;
                                    tourCategoryDTO.AverageXScore += twitterReport.Score;
                                }
                                break;
                        }
                    }

                    foreach (var postMetric in postMetrics)
                    {
                        if (postCategories.TryGetValue(postMetric.PostCategoryId, out ReportSocialMediaPostCategoryDTO postCategoryDTO))
                        {
                            postCategoryDTO.TotalSitePost++;
                            postCategoryDTO.AverageSitePostScore += postMetric.Score;
                        }
                    }
                    foreach (var attractionMetric in attractionMetrics)
                    {
                        if (attractionCategories.TryGetValue(attractionMetric.AttractionCategoryId, out ReportSocialMediaAttractionCategoryDTO attractionCategoryDTO))
                        {
                            attractionCategoryDTO.TotalAttraction++;
                            attractionCategoryDTO.AverageAttractionScore += attractionMetric.Score;
                        }
                    }
                    foreach (var tourTemplateMetric in tourTemplateMetrics)
                    {
                        if (tourCategories.TryGetValue(tourTemplateMetric.TourCategoryId, out ReportSocialMediaTourCategoryDTO tourCategoryDTO))
                        {
                            tourCategoryDTO.TotalTourTemplate++;
                            tourCategoryDTO.AverageTourTemplateScore += tourTemplateMetric.Score;
                        }
                    }
                    foreach (var postCategory in postCategories.Values)
                    {
                        postCategory.AverageFacebookScore = postCategory.TotalFacebookPost == 0 ? 0 : postCategory.AverageFacebookScore / postCategory.TotalFacebookPost;
                        postCategory.AverageXScore = postCategory.TotalXPost == 0 ? 0 : postCategory.AverageXScore / postCategory.TotalXPost;
                        postCategory.AverageSitePostScore = postCategory.TotalSitePost == 0 ? 0 : postCategory.AverageSitePostScore / postCategory.TotalSitePost;
                        postCategory.AverageScore = (postCategory.AverageFacebookScore + postCategory.AverageXScore + postCategory.AverageSitePostScore) / 3;
                    }
                    foreach (var attractionCategory in attractionCategories.Values)
                    {
                        attractionCategory.AverageFacebookScore = attractionCategory.TotalFacebookPost == 0 ? 0 : attractionCategory.AverageFacebookScore / attractionCategory.TotalFacebookPost;
                        attractionCategory.AverageXScore = attractionCategory.TotalXPost == 0 ? 0 : attractionCategory.AverageXScore / attractionCategory.TotalXPost;
                        attractionCategory.AverageAttractionScore = attractionCategory.TotalAttraction == 0 ? 0 : attractionCategory.AverageAttractionScore / attractionCategory.TotalAttraction;
                        attractionCategory.AverageScore = (attractionCategory.AverageFacebookScore + attractionCategory.AverageXScore + attractionCategory.AverageAttractionScore) / 3;
                    }
                    foreach (var tourCategory in tourCategories.Values)
                    {
                        tourCategory.AverageFacebookScore = tourCategory.TotalFacebookPost == 0 ? 0 : tourCategory.AverageFacebookScore / tourCategory.TotalFacebookPost;
                        tourCategory.AverageXScore = tourCategory.TotalXPost == 0 ? 0 : tourCategory.AverageXScore / tourCategory.TotalXPost;
                        tourCategory.AverageTourTemplateScore = tourCategory.TotalTourTemplate == 0 ? 0 : tourCategory.AverageTourTemplateScore / tourCategory.TotalTourTemplate;
                        tourCategory.AverageScore = (tourCategory.AverageFacebookScore + tourCategory.AverageXScore + tourCategory.AverageTourTemplateScore) / 3;
                    }
                    report.AttractionCategories = attractionCategories.Values.ToList();
                    report.PostCategories = postCategories.Values.ToList();
                    report.TourTemplateCategories = tourCategories.Values.ToList();


                    return report;
                }
                #endregion

                #region Category report detail

                public async Task<ReportSocialMediaAttractionCategoryDetailDTO> GetSocialMediaAttractionCategoryDetailReport(DateTime startDate, DateTime endDate, string attractionCategoryId)
                {
                    NormalizePeriod(ref startDate, ref endDate);
                    ReportSocialMediaAttractionCategoryDetailDTO report = await _unitOfWork.AttractionCategoryRepository.Query()
                        .Where(x => x.AttractionCategoryId == attractionCategoryId)
                        .Select(x => new ReportSocialMediaAttractionCategoryDetailDTO
                        {
                            AttractionCategoryId = x.AttractionCategoryId,
                            AttractionCategoryName = x.Name,
                        })
                        .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("NOT_EXIST_ATTRACTION_CATEGORY");
                    var allAttractionIds = await _unitOfWork.SocialMediaPostRepository
                        .Query()
                        .Where(x => (x.FacebookPostMetrics.Any(f => f.CreatedAt >= startDate && f.CreatedAt <= endDate) ||
                                    x.TwitterPostMetrics.Any(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)) &&
                                    x.EntityType == SocialMediaPostEntity.Attraction)
                        .Select(x => new
                        {
                            x.EntityId,
                            x.EntityType,
                        })
                        .Distinct()
                        .ToListAsync();
                    List<string> attractionIds = allAttractionIds.Select(x => x.EntityId).ToList();
                    var attractions = await _unitOfWork.AttractionRepository
                        .Query()
                        .Where(x => attractionIds.Contains(x.AttractionId) && x.AttractionCategoryId.Equals(attractionCategoryId))
                        .Select(x => new { x.AttractionId, x.ProvinceId })
                        .Distinct()
                        .ToDictionaryAsync(x => x.AttractionId!, x => x.ProvinceId!);

                    var facebookReports = await _unitOfWork.FacebookPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && attractions.Keys.Contains(x.SocialMediaPost.EntityId))
                        .Select(x => new
                        {
                            x.SocialPostId,
                            x.SocialMediaPost.EntityId,
                            x.Score,
                            x.CommentCount,
                            x.ImpressionCount,
                            x.ShareCount,
                            x.CreatedAt,
                            ReactionCount = x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount,
                        }).ToListAsync();

                    var twitterReports = await _unitOfWork.TwitterPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && attractions.Keys.Contains(x.SocialMediaPost.EntityId))
                        .Select(x => new
                        {
                            x.SocialPostId,
                            x.SocialMediaPost.EntityId,
                            x.Score,
                            x.ImpressionCount,
                            x.LikeCount,
                            x.ReplyCount,
                            x.CreatedAt,
                            RetweetCount = x.QuoteCount + x.RetweetCount,
                        }).ToListAsync();
                    var attractionMetrics = await _unitOfWork.AttractionMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.Attraction.AttractionCategoryId == attractionCategoryId)
                        .Select(x => new
                        {
                            x.AttractionId,
                            x.Score,
                            x.Attraction.ProvinceId
                        }).ToListAsync();
                    Dictionary<string, ReportSocialMediaProvinceAttractionCategoryDTO> provinces = (await _unitOfWork.ProvinceRepository
                        .Query()
                        .Select(x => new { x.ProvinceId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.ProvinceId!,
                            x => new ReportSocialMediaProvinceAttractionCategoryDTO
                            {
                                ProvinceId = x.ProvinceId,
                                ProvinceName = x.Name,
                                AverageFacebookScore = 0,
                                AverageAttractionScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalAttraction = 0,
                                TotalXPost = 0
                            }
                        );

                    report.TotalXPost = twitterReports.DistinctBy(x => x.SocialPostId).Count();
                    report.TotalAttraction = attractionMetrics.DistinctBy(x => x.AttractionId).Count();
                    report.TotalFacebookPost = facebookReports.DistinctBy(x => x.SocialPostId).Count();
                    report.AverageXScore = twitterReports.GroupBy(x => x.SocialPostId).Select(g => g.Sum(x => x.Score)).Average()??0;
                    report.AverageAttractionScore = attractionMetrics.GroupBy(x => x.AttractionId).Select(g => g.Sum(x => x.Score)).Average()??0;
                    report.AverageFacebookScore = facebookReports.GroupBy(x => x.SocialPostId).Select(g => g.Sum(x => x.Score)).Average()??0;
                    report.AverageScore = (report.AverageXScore + report.AverageAttractionScore + report.AverageFacebookScore) / 3;

                    report.ReportSocialMediaSummary = new()
                    {
                        Dates = GetPeriodLabels(startDate, endDate),
                        FacebookComments = [],
                        FacebookImpressions = [],
                        FacebookReactions = [],
                        FacebookScore = [],
                        FacebookShares = [],
                        XImpressions = [],
                        XLikes = [],
                        XReplies = [],
                        XRetweets = [],
                        XScore = [],
                    };
                    switch (GetPeriod(startDate, endDate))
                    {
                        case ReportPeriod.Daily:
                            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Monthly:
                            DateTime monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                            DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
                            for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Quarterly:
                            DateTime quarterStar = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                            DateTime quarterEnd = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1);
                            for (DateTime date = quarterStar; date <= quarterEnd; date = date.AddMonths(3))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Yearly:
                            for (int year = startDate.Year; year <= endDate.Year; year++)
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                    }

                    foreach (var facebookReport in facebookReports.GroupBy(x => new { x.SocialPostId }).Select(g => new { EntityId = g.Select(x => x.EntityId).Distinct().Single(), Score = g.Sum(x => x.Score) }).ToList())
                    {
                        if (attractions.TryGetValue(facebookReport.EntityId, out string provinceId))
                        {
                            if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvinceAttractionCategoryDTO provinceDTO))
                            {
                                provinceDTO.TotalFacebookPost++;
                                provinceDTO.AverageFacebookScore += facebookReport.Score;
                            }
                        }
                    }
                    foreach (var twitterReport in twitterReports.GroupBy(x => new { x.SocialPostId }).Select(g => new { EntityId = g.Select(x => x.EntityId).Distinct().Single(), Score = g.Sum(x => x.Score) }).ToList())
                    {
                        if (attractions.TryGetValue(twitterReport.EntityId, out string provinceId))
                        {
                            if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvinceAttractionCategoryDTO provinceDTO))
                            {
                                provinceDTO.TotalXPost++;
                                provinceDTO.AverageXScore += twitterReport.Score;
                            }
                        }
                    }
                    foreach (var attractionMetric in attractionMetrics)
                    {
                        if (provinces.TryGetValue(attractionMetric.ProvinceId, out ReportSocialMediaProvinceAttractionCategoryDTO provinceDTO))
                        {
                            provinceDTO.TotalAttraction++;
                            provinceDTO.AverageAttractionScore += attractionMetric.Score;
                        }
                    }

                    foreach (var province in provinces.Values)
                    {
                        province.AverageFacebookScore = province.TotalFacebookPost == 0 ? 0 : province.AverageFacebookScore / province.TotalFacebookPost;
                        province.AverageXScore = province.TotalXPost == 0 ? 0 : province.AverageXScore / province.TotalXPost;
                        province.AverageAttractionScore = province.TotalAttraction == 0 ? 0 : province.AverageAttractionScore / province.TotalAttraction;
                        province.AverageScore = (province.AverageFacebookScore + province.AverageXScore + province.AverageAttractionScore) / 3;
                    }
                    report.Provinces = provinces.Values.ToList();
                    return report;
                }
                public async Task<ReportSocialMediaPostCategoryDetailDTO> GetSocialMediaPostCategoryDetailReport(DateTime startDate, DateTime endDate, string postCategoryId)
                {
                    NormalizePeriod(ref startDate, ref endDate);
                    ReportSocialMediaPostCategoryDetailDTO report = await _unitOfWork.PostCategoryRepository.Query()
                        .Where(x => x.PostCategoryId == postCategoryId)
                        .Select(x => new ReportSocialMediaPostCategoryDetailDTO
                        {
                            PostCategoryId = x.PostCategoryId,
                            PostCategoryName = x.Name,
                        })
                        .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("NOT_EXIST_POST_CATEGORY");
                    var allPostIds = await _unitOfWork.SocialMediaPostRepository
                        .Query()
                        .Where(x => (x.FacebookPostMetrics.Any(f => f.CreatedAt >= startDate && f.CreatedAt <= endDate) ||
                                    x.TwitterPostMetrics.Any(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)) &&
                                    x.EntityType == SocialMediaPostEntity.Post)
                        .Select(x => new
                        {
                            x.EntityId,
                            x.EntityType,
                        })
                        .Distinct()
                        .ToListAsync();
                    List<string> postIds = allPostIds.Select(x => x.EntityId).ToList();
                    var posts = await _unitOfWork.PostRepository
                        .Query()
                        .Where(x => postIds.Contains(x.PostId) && x.PostCategoryId.Equals(postCategoryId))
                        .Select(x => new { x.PostId, x.ProvinceId })
                        .Distinct()
                        .ToDictionaryAsync(x => x.PostId!, x => x.ProvinceId!);

                    var facebookReports = await _unitOfWork.FacebookPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && posts.Keys.Contains(x.SocialMediaPost.EntityId))
                        .Select(x => new
                        {
                            x.SocialPostId,
                            x.SocialMediaPost.EntityId,
                            x.Score,
                            x.CommentCount,
                            x.ImpressionCount,
                            x.ShareCount,
                            x.CreatedAt,
                            ReactionCount = x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount,
                        }).ToListAsync();

                    var twitterReports = await _unitOfWork.TwitterPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && posts.Keys.Contains(x.SocialMediaPost.EntityId))
                        .Select(x => new
                        {
                            x.SocialPostId,
                            x.SocialMediaPost.EntityId,
                            x.Score,
                            x.ImpressionCount,
                            x.LikeCount,
                            x.ReplyCount,
                            x.CreatedAt,
                            RetweetCount = x.QuoteCount + x.RetweetCount,
                        }).ToListAsync();
                    var postMetrics = await _unitOfWork.PostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.Post.PostCategoryId == postCategoryId)
                        .Select(x => new
                        {
                            x.PostId,
                            x.Score,
                            x.Post.ProvinceId
                        }).ToListAsync();
                    Dictionary<string, ReportSocialMediaProvincePostCategoryDTO> provinces = (await _unitOfWork.ProvinceRepository
                        .Query()
                        .Select(x => new { x.ProvinceId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.ProvinceId!,
                            x => new ReportSocialMediaProvincePostCategoryDTO
                            {
                                ProvinceId = x.ProvinceId,
                                ProvinceName = x.Name,
                                AverageFacebookScore = 0,
                                AverageSitePostScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalSitePost = 0,
                                TotalXPost = 0
                            }
                        );

                    report.TotalXPost = twitterReports.DistinctBy(x => x.SocialPostId).Count();
                    report.TotalSitePost = postMetrics.DistinctBy(x => x.PostId).Count();
                    report.TotalFacebookPost = facebookReports.DistinctBy(x => x.SocialPostId).Count();
                    report.AverageXScore = twitterReports.GroupBy(x => x.SocialPostId).Select(g => g.Sum(x => x.Score)).Average() ?? 0;
                    report.AverageSitePostScore = postMetrics.GroupBy(x => x.PostId).Select(g => g.Sum(x => x.Score)).Average() ?? 0;
                    report.AverageFacebookScore = facebookReports.GroupBy(x => x.SocialPostId).Select(g => g.Sum(x => x.Score)).Average() ?? 0;
                    report.AverageScore = (report.AverageXScore + report.AverageSitePostScore + report.AverageFacebookScore) / 3;

                    report.ReportSocialMediaSummary = new()
                    {
                        Dates = GetPeriodLabels(startDate, endDate),
                        FacebookComments = [],
                        FacebookImpressions = [],
                        FacebookReactions = [],
                        FacebookScore = [],
                        FacebookShares = [],
                        XImpressions = [],
                        XLikes = [],
                        XReplies = [],
                        XRetweets = [],
                        XScore = [],
                    };
                    switch (GetPeriod(startDate, endDate))
                    {
                        case ReportPeriod.Daily:
                            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Monthly:
                            DateTime monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                            DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
                            for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Quarterly:
                            DateTime quarterStar = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                            DateTime quarterEnd = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1);
                            for (DateTime date = quarterStar; date <= quarterEnd; date = date.AddMonths(3))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Yearly:
                            for (int year = startDate.Year; year <= endDate.Year; year++)
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                    }

                    foreach (var facebookReport in facebookReports.GroupBy(x => new { x.SocialPostId }).Select(g => new { EntityId = g.Select(x => x.EntityId).Distinct().Single(), Score = g.Sum(x => x.Score) }).ToList())
                    {
                        if (posts.TryGetValue(facebookReport.EntityId, out string provinceId))
                        {
                            if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvincePostCategoryDTO provinceDTO))
                            {
                                provinceDTO.TotalFacebookPost++;
                                provinceDTO.AverageFacebookScore += facebookReport.Score;
                            }
                        }
                    }
                    foreach (var twitterReport in twitterReports.GroupBy(x => new { x.SocialPostId }).Select(g => new { EntityId = g.Select(x => x.EntityId).Distinct().Single(), Score = g.Sum(x => x.Score) }).ToList())
                    {
                        if (posts.TryGetValue(twitterReport.EntityId, out string provinceId))
                        {
                            if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvincePostCategoryDTO provinceDTO))
                            {
                                provinceDTO.TotalXPost++;
                                provinceDTO.AverageXScore += twitterReport.Score;
                            }
                        }
                    }
                    foreach (var postMetric in postMetrics)
                    {
                        if (provinces.TryGetValue(postMetric.ProvinceId, out ReportSocialMediaProvincePostCategoryDTO provinceDTO))
                        {
                            provinceDTO.TotalSitePost++;
                            provinceDTO.AverageSitePostScore += postMetric.Score;
                        }
                    }

                    foreach (var province in provinces.Values)
                    {
                        province.AverageFacebookScore = province.TotalFacebookPost == 0 ? 0 : province.AverageFacebookScore / province.TotalFacebookPost;
                        province.AverageXScore = province.TotalXPost == 0 ? 0 : province.AverageXScore / province.TotalXPost;
                        province.AverageSitePostScore = province.TotalSitePost == 0 ? 0 : province.AverageSitePostScore / province.TotalSitePost;
                        province.AverageScore = (province.AverageFacebookScore + province.AverageXScore + province.AverageSitePostScore) / 3;
                    }
                    report.Provinces = provinces.Values.ToList();
                    return report;
                }
                public async Task<ReportSocialMediaTourCategoryDetailDTO> GetSocialMediaTourCategoryDetailReport(DateTime startDate, DateTime endDate, string tourCategoryId)
                {
                    NormalizePeriod(ref startDate, ref endDate);
                    ReportSocialMediaTourCategoryDetailDTO report = await _unitOfWork.TourCategoryRepository.Query()
                        .Where(x => x.TourCategoryId == tourCategoryId)
                        .Select(x => new ReportSocialMediaTourCategoryDetailDTO
                        {
                            TourCategoryId = x.TourCategoryId,
                            TourCategoryName = x.Name,
                        })
                        .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("NOT_EXIST_POST_CATEGORY");
                    var allTourTemplateIds = await _unitOfWork.SocialMediaPostRepository
                        .Query()
                        .Where(x => (x.FacebookPostMetrics.Any(f => f.CreatedAt >= startDate && f.CreatedAt <= endDate) ||
                                    x.TwitterPostMetrics.Any(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)) &&
                                    x.EntityType == SocialMediaPostEntity.Post)
                        .Select(x => new
                        {
                            x.EntityId,
                            x.EntityType,
                        })
                        .Distinct()
                        .ToListAsync();
                    List<string> tourTemplateIds = allTourTemplateIds.Select(x => x.EntityId).ToList();
                    var tourTemplates = await _unitOfWork.TourTemplateRepository
                        .Query()
                        .Where(x => tourTemplateIds.Contains(x.TourTemplateId) && x.TourCategoryId.Equals(tourCategoryId))
                        .Select(x => new { x.TourTemplateId, ProvinceIds = x.TourTemplateProvinces.Select(x=>x.ProvinceId).ToList()})
                        .ToDictionaryAsync(x => x.TourTemplateId!, x => x.ProvinceIds!);

                    var facebookReports = await _unitOfWork.FacebookPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && tourTemplates.Keys.Contains(x.SocialMediaPost.EntityId))
                        .Select(x => new
                        {
                            x.SocialPostId,
                            x.SocialMediaPost.EntityId,
                            x.Score,
                            x.CommentCount,
                            x.ImpressionCount,
                            x.ShareCount,
                            x.CreatedAt,
                            ReactionCount = x.LikeCount + x.LoveCount + x.WowCount + x.HahaCount + x.SorryCount + x.AngerCount,
                        }).ToListAsync();

                    var twitterReports = await _unitOfWork.TwitterPostMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && tourTemplates.Keys.Contains(x.SocialMediaPost.EntityId))
                        .Select(x => new
                        {
                            x.SocialPostId,
                            x.SocialMediaPost.EntityId,
                            x.Score,
                            x.ImpressionCount,
                            x.LikeCount,
                            x.ReplyCount,
                            x.CreatedAt,
                            RetweetCount = x.QuoteCount + x.RetweetCount,
                        }).ToListAsync();
                    var tourTemplateMetrics = await _unitOfWork.TourTemplateMetricRepository
                        .Query()
                        .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && x.TourTemplate.TourCategoryId == tourCategoryId)
                        .Select(x => new
                        {
                            x.TourTemplateId,
                            x.Score,
                            ProvinceIds = x.TourTemplate.TourTemplateProvinces.Select(x => x.ProvinceId).ToList()
                        }).ToListAsync();
                    Dictionary<string, ReportSocialMediaProvinceTourCategoryDTO> provinces = (await _unitOfWork.ProvinceRepository
                        .Query()
                        .Select(x => new { x.ProvinceId, x.Name })
                        .ToListAsync())
                        .ToDictionary(
                            x => x.ProvinceId!,
                            x => new ReportSocialMediaProvinceTourCategoryDTO
                            {
                                ProvinceId = x.ProvinceId,
                                ProvinceName = x.Name,
                                AverageFacebookScore = 0,
                                AverageTourTemplateScore = 0,
                                AverageScore = 0,
                                AverageXScore = 0,
                                TotalFacebookPost = 0,
                                TotalTourTemplate = 0,
                                TotalXPost = 0
                            }
                        );

                    report.TotalXPost = twitterReports.DistinctBy(x => x.SocialPostId).Count();
                    report.TotalTourTemplate = tourTemplateMetrics.DistinctBy(x => x.TourTemplateId).Count();
                    report.TotalFacebookPost = facebookReports.DistinctBy(x => x.SocialPostId).Count();
                    report.AverageXScore = twitterReports.GroupBy(x => x.SocialPostId).Select(g => g.Sum(x => x.Score)).Average() ?? 0;
                    report.AverageTourTemplateScore = tourTemplateMetrics.GroupBy(x => x.TourTemplateId).Select(g => g.Sum(x => x.Score)).Average() ?? 0;
                    report.AverageFacebookScore = facebookReports.GroupBy(x => x.SocialPostId).Select(g => g.Sum(x => x.Score)).Average() ?? 0;
                    report.AverageScore = (report.AverageXScore + report.AverageTourTemplateScore + report.AverageFacebookScore) / 3;

                    report.ReportSocialMediaSummary = new()
                    {
                        Dates = GetPeriodLabels(startDate, endDate),
                        FacebookComments = [],
                        FacebookImpressions = [],
                        FacebookReactions = [],
                        FacebookScore = [],
                        FacebookShares = [],
                        XImpressions = [],
                        XLikes = [],
                        XReplies = [],
                        XRetweets = [],
                        XScore = [],
                    };
                    switch (GetPeriod(startDate, endDate))
                    {
                        case ReportPeriod.Daily:
                            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddDays(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Monthly:
                            DateTime monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                            DateTime monthEnd = new DateTime(endDate.Year, endDate.Month, 1).AddMonths(1).AddDays(-1);
                            for (DateTime date = monthStart; date <= monthEnd; date = date.AddMonths(1))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(1))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Quarterly:
                            DateTime quarterStar = new DateTime(startDate.Year, ((startDate.Month - 1) / 3) * 3 + 1, 1);
                            DateTime quarterEnd = new DateTime(endDate.Year, ((endDate.Month - 1) / 3) * 3 + 1, 1);
                            for (DateTime date = quarterStar; date <= quarterEnd; date = date.AddMonths(3))
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt >= date && x.CreatedAt < date.AddMonths(3))
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                        case ReportPeriod.Yearly:
                            for (int year = startDate.Year; year <= endDate.Year; year++)
                            {
                                var facebookMetrics = facebookReports
                                    .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalComments = g.Sum(x => x.CommentCount),
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalShares = g.Sum(x => x.ShareCount),
                                        TotalReactions = g.Sum(x => x.ReactionCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.FacebookComments.Add(facebookMetrics?.TotalComments ?? 0);
                                report.ReportSocialMediaSummary.FacebookImpressions.Add(facebookMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.FacebookShares.Add(facebookMetrics?.TotalShares ?? 0);
                                report.ReportSocialMediaSummary.FacebookReactions.Add(facebookMetrics?.TotalReactions ?? 0);
                                report.ReportSocialMediaSummary.FacebookScore.Add(facebookMetrics?.TotalScore ?? 0);
                                var twitterMetrics = twitterReports
                                    .Where(x => x.CreatedAt.Year >= year && x.CreatedAt.Year < year + 1)
                                    .GroupBy(x => 1)
                                    .Select(g => new
                                    {
                                        TotalImpressions = g.Sum(x => x.ImpressionCount),
                                        TotalLikes = g.Sum(x => x.LikeCount),
                                        TotalReplies = g.Sum(x => x.ReplyCount),
                                        TotalRetweets = g.Sum(x => x.RetweetCount),
                                        TotalScore = g.Sum(x => x.Score)
                                    })
                                    .FirstOrDefault();
                                report.ReportSocialMediaSummary.XImpressions.Add(twitterMetrics?.TotalImpressions ?? 0);
                                report.ReportSocialMediaSummary.XLikes.Add(twitterMetrics?.TotalLikes ?? 0);
                                report.ReportSocialMediaSummary.XReplies.Add(twitterMetrics?.TotalReplies ?? 0);
                                report.ReportSocialMediaSummary.XRetweets.Add(twitterMetrics?.TotalRetweets ?? 0);
                                report.ReportSocialMediaSummary.XScore.Add(twitterMetrics?.TotalScore ?? 0);
                            }
                            break;
                    }

                    foreach (var facebookReport in facebookReports.GroupBy(x => new { x.SocialPostId }).Select(g => new { EntityId = g.Select(x => x.EntityId).Distinct().Single(), Score = g.Sum(x => x.Score) }).ToList())
                    {
                        if (tourTemplates.TryGetValue(facebookReport.EntityId, out List<string> provinceIds))
                        {
                            foreach (var provinceId in provinceIds)
                            {
                                if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvinceTourCategoryDTO provinceDTO))
                                {
                                    provinceDTO.TotalFacebookPost++;
                                    provinceDTO.AverageFacebookScore += facebookReport.Score;
                                }
                            }
                        }
                    }
                    foreach (var twitterReport in twitterReports.GroupBy(x => new { x.SocialPostId }).Select(g => new { EntityId = g.Select(x => x.EntityId).Distinct().Single(), Score = g.Sum(x => x.Score) }).ToList())
                    {
                        if (tourTemplates.TryGetValue(twitterReport.EntityId, out List<string> provinceIds))
                        {
                            foreach (var provinceId in provinceIds)
                            {
                                if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvinceTourCategoryDTO provinceDTO))
                                {
                                    provinceDTO.TotalXPost++;
                                    provinceDTO.AverageXScore += twitterReport.Score;
                                }
                            }
                        }
                    }
                    foreach (var tourTemplateMetric in tourTemplateMetrics)
                    {
                        foreach (var provinceId in tourTemplateMetric.ProvinceIds)
                        {
                            if (provinces.TryGetValue(provinceId, out ReportSocialMediaProvinceTourCategoryDTO provinceDTO))
                            {
                                provinceDTO.TotalTourTemplate++;
                                provinceDTO.AverageTourTemplateScore += tourTemplateMetric.Score;
                            }
                        }
                    }

                    foreach (var province in provinces.Values)
                    {
                        province.AverageFacebookScore = province.TotalFacebookPost == 0 ? 0 : province.AverageFacebookScore / province.TotalFacebookPost;
                        province.AverageXScore = province.TotalXPost == 0 ? 0 : province.AverageXScore / province.TotalXPost;
                        province.AverageTourTemplateScore = province.TotalTourTemplate == 0 ? 0 : province.AverageTourTemplateScore / province.TotalTourTemplate;
                        province.AverageScore = (province.AverageFacebookScore + province.AverageXScore + province.AverageTourTemplateScore) / 3;
                    }
                    report.Provinces = provinces.Values.ToList();
                    return report;
                }

                #endregion

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
                        <= 31 => ReportPeriod.Daily,
                        <= 365 => ReportPeriod.Monthly,
                        <= 1095 => ReportPeriod.Quarterly,
                        _ => ReportPeriod.Yearly
                    };
                }
                private void NormalizePeriod(ref DateTime startDate, ref DateTime endDate)
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
                enum ReportPeriod
                {
                    Daily,
                    Monthly,
                    Quarterly,
                    Yearly
                }
                #endregion
        */
    }
}