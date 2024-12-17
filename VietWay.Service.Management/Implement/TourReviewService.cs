using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Service.Management.Implement
{
    public class TourReviewService(IUnitOfWork unitOfWork) : ITourReviewService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<(int count, List<TourReviewDTO>)> GetTourReviewsAsync(string tourTemplateId, List<int>? ratingValue, bool? hasReviewContent, int pageSize, int pageIndex, bool? isDeleted)
        {
            IQueryable<TourReview> query = _unitOfWork.TourReviewRepository.Query()
                .Where(x => x.Booking.Tour.TourTemplateId.Equals(tourTemplateId));
            if (ratingValue?.Count > 0)
            {
                query = query.Where(x => ratingValue.Contains(x.Rating));
            }
            if (hasReviewContent.HasValue && true == hasReviewContent.Value)
            {
                query = query.Where(x => null != x.Review);
            }
            if (isDeleted.HasValue)
            {
                query = query.Where(x => x.IsDeleted == isDeleted);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }
            int count = await query.CountAsync();
            List<TourReviewDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TourReviewDTO
                {
                    ReviewId = x.ReviewId,
                    Rating = x.Rating,
                    Review = x.Review,
                    CreatedAt = x.CreatedAt,
                    Reviewer = x.Booking.CustomerInfo!.FullName,
                    IsDeleted = x.IsDeleted
                })
                .ToListAsync();
            return (count, items);
        }

        public async Task ToggleTourReviewVisibilityAsync(string accountId, string reviewId, bool isHided, string reason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Account? account = await _unitOfWork.AccountRepository.Query()
                    .SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)) ??
                    throw new UnauthorizedException("UNAUTHORIZED");
                TourReview review = _unitOfWork.TourReviewRepository.Query()
                    .SingleOrDefault(x => x.ReviewId.Equals(reviewId)) ??
                    throw new ResourceNotFoundException("NOT_EXIST_REVIEW");
                review.IsDeleted = isHided;
                await _unitOfWork.TourReviewRepository.UpdateAsync(review);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
