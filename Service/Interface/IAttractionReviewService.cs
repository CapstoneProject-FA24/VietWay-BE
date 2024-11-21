using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IAttractionReviewService
    {
        public Task<(int count, List<AttractionReviewDTO>)> GetAttractionReviewsAsync(string attractionId, bool isOrderedByLikeNumber, List<int>? ratingValue, bool? hasReviewContent, int pageSize, int pageIndex, bool? isDeleted);
        public Task ToggleAttractionReviewVisibilityAsync(string accountId, string reviewId, bool isHided, string reason);
    }
}
