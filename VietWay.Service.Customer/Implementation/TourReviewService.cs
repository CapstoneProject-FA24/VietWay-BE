using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.Configuration;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class TourReviewService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper,
        TourReviewServiceConfiguration config) : ITourReviewService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly int _reviewTourExpireAfterDays = config.ReviewTourExpireAfterDays;

        public async Task CreateTourReviewAsync(string customerId, TourReview tourReview)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Booking booking = await _unitOfWork.BookingRepository.Query()
                    .Include(x => x.Tour.TourTemplate.TourDuration)
                    .SingleOrDefaultAsync(x => x.BookingId == tourReview.BookingId && x.CustomerId == customerId && x.Status == BookingStatus.Completed)
                    ?? throw new ResourceNotFoundException("NOT_EXIST_BOOKING");
                if (booking.Tour!.StartDate!.Value.AddDays(booking.Tour!.TourTemplate!.TourDuration!.NumberOfDay).AddDays(_reviewTourExpireAfterDays) < _timeZoneHelper.GetUTC7Now())
                {
                    throw new InvalidActionException("INVALID_ACTION_REVIEW_EXPIRED");
                }
                tourReview.ReviewId = _idGenerator.GenerateId();
                tourReview.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.TourReviewRepository.CreateAsync(tourReview);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

        }

        public async Task<TourReviewDTO?> GetTourReviewByBookingIdAsync(string customerId, string bookingId)
        {
            return await _unitOfWork.TourReviewRepository.Query()
                .Where(x => x.Booking.CustomerId == customerId && x.Booking.BookingId == bookingId)
                .Select(Booking => new TourReviewDTO
                {
                    CreatedAt = Booking.CreatedAt,
                    Rating = Booking.Rating,
                    ReviewId = Booking.ReviewId,
                    Review = Booking.Review,
                    Reviewer = Booking.Booking.CustomerInfo.FullName,
                }).SingleOrDefaultAsync();
        }

        public async Task<PaginatedList<TourReviewDTO>> GetTourReviewsAsync(string tourTemplateId, List<int>? ratingValue, bool? hasReviewContent, int pageSize, int pageIndex)
        {
            IQueryable<TourReview> query = _unitOfWork.TourReviewRepository.Query()
                .Where(x => x.Booking.Tour.TourTemplate.TourTemplateId.Equals(tourTemplateId))
                .Where(x => x.IsPublic == true);

            if (ratingValue?.Count > 0)
            {
                query = query.Where(x => ratingValue.Contains(x.Rating));
            }
            if (hasReviewContent.HasValue && true == hasReviewContent.Value)
            {
                query = query.Where(x => null != x.Review);
            }
            int count = await query.CountAsync();

            List<TourReviewDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TourReviewDTO
                {
                    CreatedAt = x.CreatedAt,
                    Rating = x.Rating,
                    ReviewId = x.ReviewId,
                    Review = x.Review,
                    Reviewer = x.Booking.CustomerInfo.FullName
                }).ToListAsync();

            return new PaginatedList<TourReviewDTO>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = count
            };
        }
    }
}
