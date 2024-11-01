using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class PostService : IPostService
    {
        public Task<PostDetailDTO?> GetPostDetailAsync(string tourId)
        {
            throw new NotImplementedException();
        }

        public Task<(int counts, List<PostPreviewDTO> items)> GetPostPreviewsAsync(string? nameSearch, List<string>? provinceIds, List<string>? postCategoryIds, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }
    }
}
