﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IPostService
    {
        Task<PostDetailDTO?> GetPostDetailAsync(string tourId);
        public Task<(int counts,List<PostPreviewDTO> items)> GetPostPreviewsAsync(string? nameSearch, List<string>? provinceIds, 
            List<string>? postCategoryIds, int pageSize, int pageIndex);
    }
}
