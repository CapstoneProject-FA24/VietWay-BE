﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IPostService
    {
        public Task<PaginatedList<PostPreviewDTO>> GetAllPostAsync(
            string? nameSearch,
            List<string>? postCategoryIds,
            List<string>? provinceIds,
            List<PostStatus>? status,
            int pageSize,
            int pageIndex);
        public Task<string> CreatePostAsync(Post post);
        public Task DeletePostAsync(string postId);
        public Task UpdatePostAsync(Post newPost, string accountId);
        public Task<PostDetailDTO?> GetPostByIdAsync(string postId);
        public Task ChangePostStatusAsync(string postId, string accountId, PostStatus postStatus, string? reason);
        public Task UpdatePostImageAsync(string postId, IFormFile newImages);
    }
}
