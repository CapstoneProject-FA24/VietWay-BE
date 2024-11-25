using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class PostCategoryService(IUnitOfWork unitOfWork) : IPostCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<PostCategoryDTO>> GetPostCategoriesAsync()
        {
            return await _unitOfWork.PostCategoryRepository.Query()
                .Select(x => new PostCategoryDTO()
                {
                    Description = x.Description,
                    Name = x.Name,
                    PostCategoryId = x.PostCategoryId,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
        }
    }
}
