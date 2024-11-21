using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourReviewService
    {
        public Task<(int count, List<TourReviewDTO>)> GetTourReviewsAsync(string tourTemplateId, List<int>? ratingValue, bool? hasReviewContent, int pageSize, int pageIndex, bool? isDeleted);
        public Task ToggleTourReviewVisibilityAsync(string accountId, string reviewId, bool isHided, string reason);
    }
}
