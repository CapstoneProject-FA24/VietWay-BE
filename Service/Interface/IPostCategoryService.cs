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
        public Task<List<PostCategoryDTO>> GetPostCategoriesAsync();
        public Task<string> CreatePostCategoryAsync(PostCategory postCategory);
    }
}
