using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class PostCategoryService(IUnitOfWork unitOfWork) : IPostCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<PostCategoryDTO>> GetPostCategoriesAsync()
        {
            return await _unitOfWork.PostCategoryRepository.Query()
                .Where(x => false == x.IsDeleted)
                .Select(x => new PostCategoryDTO
                {
                    PostCategoryId = x.PostCategoryId,
                    Name = x.Name,
                    Description = x.Description
                }).ToListAsync();
        }
    }
}
