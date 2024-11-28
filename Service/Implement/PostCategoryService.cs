using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class PostCategoryService(IUnitOfWork unitOfWork,
        IIdGenerator idGenerator) : IPostCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
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
        public async Task<string> CreatePostCategoryAsync(PostCategory postCategory)
        {
            postCategory.CreatedAt = DateTime.Now;
            try
            {
                postCategory.PostCategoryId ??= _idGenerator.GenerateId();
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.PostCategoryRepository.CreateAsync(postCategory);
                await _unitOfWork.CommitTransactionAsync();
                return postCategory.PostCategoryId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
