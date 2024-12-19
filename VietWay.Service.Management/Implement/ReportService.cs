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

            List<ReportBookingParticipantCountDTO> participantCounts = await _unitOfWork.BookingRepository.Query()
                .Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .GroupBy(x => x.BookingTourists.Count)
                .Select(x => new ReportBookingParticipantCountDTO
                {
                    ParticipantCount = x.Key,
                    BookingCount = x.Count()
                })
                .ToListAsync();
            List<ReportBookingByTourCategory> categoryBookings = await _unitOfWork.TourCategoryRepository.Query()
                .Select(x => new ReportBookingByTourCategory
                {
                    TourCategoryName = x.Name,
                    TotalBooking = x.TourTemplates.SelectMany(t => t.Tours).SelectMany(t => t.TourBookings)
                        .Where(b => b.CreatedAt >= startDate && b.CreatedAt <= endDate).Count()
                })
                .ToListAsync();
            List<ReportBookingByTourTemplate> templateBookings = await _unitOfWork.TourTemplateRepository.Query()
                .Select(x => new ReportBookingByTourTemplate
                {
                    TourTemplateName = x.TourName,
                    TotalBooking = x.Tours.SelectMany(t => t.TourBookings)
                        .Where(b => b.CreatedAt >= startDate && b.CreatedAt <= endDate).Count()
                })
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

            IQueryable<AttractionRatingDTO> attractions = _unitOfWork.AttractionRepository.Query().Select(x=>new AttractionRatingDTO
            {
                AttractionName = x.Name,
                AverageRating = x.AttractionReviews.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).Average(r => r.Rating),
                TotalRating = x.AttractionReviews.Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).Count()
            });
            IQueryable<TourTemplateRatingDTO> tourTemplates = _unitOfWork.TourTemplateRepository.Query().Select(x => new TourTemplateRatingDTO
            {
                TourTemplateName = x.TourName,
                AverageRating = x.Tours.SelectMany(t => t.TourBookings).Select(x=>x.TourReview).Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).Average(r => r.Rating),
                TotalRating = x.Tours.SelectMany(t => t.TourBookings).Select(x => x.TourReview).Where(r => r.CreatedAt >= startDate && r.CreatedAt <= endDate).Count()
            });
            attractions = isAsc ? attractions.OrderBy(x => x.AverageRating) : attractions.OrderByDescending(x => x.AverageRating);
            tourTemplates = isAsc ? tourTemplates.OrderBy(x => x.AverageRating) : tourTemplates.OrderByDescending(x => x.AverageRating);

            IQueryable<AttractionRatingDTO> attractionTotal = _unitOfWork.AttractionRepository.Query().Select(x => new AttractionRatingDTO
            {
                AttractionName = x.Name,
                AverageRating = x.AttractionReviews.Average(r => r.Rating),
                TotalRating = x.AttractionReviews.Count()
            });
            IQueryable<TourTemplateRatingDTO> tourTemplateTotal = _unitOfWork.TourTemplateRepository.Query().Select(x => new TourTemplateRatingDTO
            {
                TourTemplateName = x.TourName,
                AverageRating = x.Tours.SelectMany(t => t.TourBookings).Select(x => x.TourReview).Average(r => r.Rating),
                TotalRating = x.Tours.SelectMany(t => t.TourBookings).Select(x => x.TourReview).Count()
            });
            attractionTotal = isAsc ? attractions.OrderBy(x => x.AverageRating) : attractions.OrderByDescending(x => x.AverageRating);
            tourTemplateTotal = isAsc ? tourTemplates.OrderBy(x => x.AverageRating) : tourTemplates.OrderByDescending(x => x.AverageRating);

            return new ReportRatingDTO
            {
                AttractionRatingInPeriod = await attractions.Take(15).ToListAsync(),
                AttractionRatingTotal = await attractionTotal.Take(15).ToListAsync(),
                TourTemplateRatingInPeriod = await tourTemplates.Take(15).ToListAsync(),
                TourTemplateRatingTotal = await tourTemplateTotal.Take(15).ToListAsync()
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
