using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IPostCategoryService
    {
        public Task<List<PostCategoryDTO>> GetPostCategoriesAsync(string? nameSearch);
        public Task<string> CreatePostCategoryAsync(PostCategory postCategory);
        public Task UpdatePostCategoryAsync(string postCategoryId, PostCategory newPostCategory);
        public Task DeletePostCategoryAsync(string postCategoryId);
        public Task<PostCategoryDTO?> GetPostCategoryByIdAsync(string postCategoryId);
    }
}
