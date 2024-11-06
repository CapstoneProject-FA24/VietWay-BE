using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IAttractionReviewService
    {
        public Task<(int count, List<AttractionReviewDTO>)> GetOtherAttractionReviewsAsync(string attractionId, string? customerId, bool isOrderedByLikeNumber, List<int>? ratingValue,
            bool? hasReviewContent, int pageSize, int pageIndex);
        public Task<AttractionReviewDTO?> GetUserAttractionReviewAsync(string attractionId, string customerId);
        public Task AddAttractionReviewAsync(AttractionReview review);
        public Task UpdateAttractionReviewAsync(AttractionReview review);
        public Task DeleteAttractionReviewAsync(string reviewId, string customerId);
        public Task ToggleAttractionReviewLikeAsync(string reviewId, string customerId, bool isLike);

    }
}
