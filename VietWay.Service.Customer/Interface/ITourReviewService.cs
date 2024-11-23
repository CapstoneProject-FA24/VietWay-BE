using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface ITourReviewService
    {
        public Task<PaginatedList<TourReviewDTO>> GetTourReviewsAsync(string tourTemplateId, List<int>? ratingValue,
            bool? hasReviewContent, int pageSize, int pageIndex);
        public Task CreateTourReviewAsync(string customerId, TourReview tourReview);
        public Task<TourReviewDTO?> GetTourReviewByBookingIdAsync(string customerId, string bookingId);
    }
}
