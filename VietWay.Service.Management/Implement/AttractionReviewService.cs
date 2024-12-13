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
    public class AttractionReviewService(IUnitOfWork unitOfWork) : IAttractionReviewService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<(int count, List<AttractionReviewDTO>)> GetAttractionReviewsAsync(string attractionId, bool isOrderedByLikeNumber, List<int>? ratingValue, bool? hasReviewContent, int pageSize, int pageIndex, bool? isDeleted)
        {
            IQueryable<AttractionReview> query = _unitOfWork.AttractionReviewRepository.Query()
                .Where(x => x.AttractionId.Equals(attractionId));
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
            if (isOrderedByLikeNumber)
            {
                query = query.OrderByDescending(x => x.AttractionReviewLikes!.Count);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }
            int count = await query.CountAsync();
            List<AttractionReviewDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new AttractionReviewDTO
                {
                    ReviewId = x.ReviewId,
                    Rating = x.Rating,
                    Review = x.Review,
                    CreatedAt = x.CreatedAt,
                    Reviewer = x.Customer!.FullName,
                    LikeCount = x.AttractionReviewLikes!.Count,
                    IsDeleted = x.IsDeleted
                })
                .ToListAsync();
            return (count, items);
        }

        public async Task ToggleAttractionReviewVisibilityAsync(string accountId, string reviewId, bool isHided, string reason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Account? account = await _unitOfWork.AccountRepository.Query()
                    .SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)) ??
                    throw new UnauthorizedException("Account not found");
                AttractionReview review = _unitOfWork.AttractionReviewRepository.Query()
                    .SingleOrDefault(x => x.ReviewId.Equals(reviewId)) ??
                    throw new ResourceNotFoundException("Attraction review not found");
                review.IsDeleted = isHided;
                await _unitOfWork.AttractionReviewRepository.UpdateAsync(review);
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
