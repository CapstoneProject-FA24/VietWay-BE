using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IPostService
    {
        Task<PostDetailDTO?> GetPostDetailAsync(string postId,string? customerId);
        public Task<PaginatedList<PostPreviewDTO>> GetCustomerLikedPostPreviewsAsync(string customerId, int pageSize, int pageIndex);
        public Task<PaginatedList<PostPreviewDTO>> GetPostPreviewsAsync(string? nameSearch, List<string>? provinceIds, 
            List<string>? postCategoryIds, string? customerId, int pageSize, int pageIndex);
        Task TogglePostLikeAsync(string postId, string? customerId, bool isLike);
    }
}
