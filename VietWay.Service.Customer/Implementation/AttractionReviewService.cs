using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class AttractionReviewService(IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper, IUnitOfWork unitOfWork) : IAttractionReviewService
    {
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task AddAttractionReviewAsync(AttractionReview review)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                review.ReviewId = _idGenerator.GenerateId();
                review.CreatedAt = _timeZoneHelper.GetUTC7Now();
                bool isExist = await _unitOfWork.AttractionReviewRepository.Query().AnyAsync(x => x.AttractionId.Equals(review.AttractionId) && x.CustomerId.Equals(review.CustomerId));
                if (isExist)
                {
                    throw new ResourceAlreadyExistsException(nameof(AttractionReview));
                }
                await _unitOfWork.AttractionReviewRepository.CreateAsync(review);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteAttractionReviewAsync(string reviewId, string customerId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                AttractionReview review = await _unitOfWork.AttractionReviewRepository.Query()
                    .SingleOrDefaultAsync(x => x.ReviewId.Equals(reviewId) && true == x.IsDeleted && x.CustomerId.Equals(customerId)) ??
                    throw new ResourceNotFoundException(nameof(AttractionReview));
                review.IsDeleted = true;
                await _unitOfWork.AttractionReviewRepository.UpdateAsync(review);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PaginatedList<AttractionReviewDTO>> GetOtherAttractionReviewsAsync(string attractionId, string? customerId, 
            bool isOrderedByLikeNumber,  List<int>? ratingValue, bool? hasReviewContent, int pageSize, int pageIndex)
        {
            IQueryable<AttractionReview> query = _unitOfWork.AttractionReviewRepository.Query()
                .Where(x => x.AttractionId.Equals(attractionId) && x.CustomerId != customerId && true != x.IsDeleted);
            if (ratingValue?.Count >0) 
            {
                query = query.Where(x => ratingValue.Contains(x.Rating));
            }
            if (hasReviewContent.HasValue && true == hasReviewContent.Value)
            {
                query = query.Where(x => null != hasReviewContent);
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
                .Skip((pageIndex-1) * pageSize)
                .Take(pageSize)
                .Select(x => new AttractionReviewDTO
                {
                    ReviewId = x.ReviewId,
                    Rating = x.Rating,
                    Review = x.Review,
                    CreatedAt = x.CreatedAt,
                    Reviewer = x.Customer!.FullName,
                    LikeCount = x.AttractionReviewLikes!.Count
                })
                .ToListAsync();
            return new PaginatedList<AttractionReviewDTO>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = count
            };
        }

        public async Task<AttractionReviewDTO?> GetUserAttractionReviewAsync(string attractionId, string customerId)
        {
            return await _unitOfWork.AttractionReviewRepository.Query()
                .Where(x => x.AttractionId.Equals(attractionId) && x.CustomerId.Equals(customerId) && false == x.IsDeleted)
                .Select(x=> new AttractionReviewDTO
                {
                    ReviewId = x.ReviewId,
                    Rating = x.Rating,
                    Review = x.Review,
                    CreatedAt = x.CreatedAt,
                    Reviewer = x.Customer!.FullName,
                    LikeCount = x.AttractionReviewLikes!.Count
                }).SingleOrDefaultAsync();
        }

        public async Task ToggleAttractionReviewLikeAsync(string reviewId, string customerId, bool isLike)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                AttractionReviewLike? attractionReviewLike = await _unitOfWork.AttractionReviewLikeRepository.Query()
                    .SingleOrDefaultAsync(x => x.ReviewId.Equals(reviewId) && x.CustomerId.Equals(customerId) && false == x.AttractionReview!.IsDeleted);
                if (null == attractionReviewLike && isLike)
                {
                    await _unitOfWork.AttractionReviewLikeRepository.CreateAsync(new AttractionReviewLike
                    {
                        CustomerId = customerId,
                        ReviewId = reviewId
                    });
                }
                else if (null != attractionReviewLike && false == isLike)
                {
                    await _unitOfWork.AttractionReviewLikeRepository.DeleteAsync(attractionReviewLike);
                }
                else
                {
                    throw new InvalidOperationException(nameof(AttractionReviewLike));
                }
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateAttractionReviewAsync(AttractionReview review)
        {
            try
            {
                await _unitOfWork.RollbackTransactionAsync();
                AttractionReview oldReview = await _unitOfWork.AttractionReviewRepository.Query()
                    .SingleOrDefaultAsync(x => x.AttractionId.Equals(review.AttractionId) && false == x.IsDeleted && x.CustomerId.Equals(review.CustomerId)) ??
                    throw new ResourceNotFoundException(nameof(AttractionReview));
                oldReview.Rating = review.Rating;
                oldReview.Review = review.Review;
                oldReview.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.AttractionReviewRepository.UpdateAsync(oldReview);
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
