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
using VietWay.Util.CustomExceptions;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class PostCategoryService(IUnitOfWork unitOfWork,
        IIdGenerator idGenerator) : IPostCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task<List<PostCategoryDTO>> GetPostCategoriesAsync(string? nameSearch)
        {
            var query = _unitOfWork
                .PostCategoryRepository
                .Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }

            return await query.Select(x => new PostCategoryDTO()
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
                var existingCategory = await GetByNameAsync(postCategory.Name);
                if (existingCategory != null)
                {
                    throw new InvalidOperationException($"A category with the name '{existingCategory.Name}' already exists.");
                }

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

        private async Task<PostCategory> GetByNameAsync(string name)
        {
            return await _unitOfWork.PostCategoryRepository.Query()
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task UpdatePostCategoryAsync(string postCategoryId, PostCategory newPostCategory)
        {
            PostCategory? postCategory = await _unitOfWork.PostCategoryRepository.Query()
                .SingleOrDefaultAsync(x => x.PostCategoryId.Equals(postCategoryId)) ??
                throw new ResourceNotFoundException("Post Category not found");

            postCategory.Name = newPostCategory.Name;
            postCategory.Description = newPostCategory.Description;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.PostCategoryRepository.UpdateAsync(postCategory);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task DeletePostCategoryAsync(string postCategoryId)
        {
            PostCategory? postCategory = await _unitOfWork.PostCategoryRepository.Query()
                .SingleOrDefaultAsync(x => x.PostCategoryId.Equals(postCategoryId)) ??
                throw new ResourceNotFoundException("Post Category not found");
            bool hasRelatedData = await _unitOfWork.PostRepository.Query().AnyAsync(x => x.PostCategoryId.Equals(postCategoryId));
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (hasRelatedData)
                {
                    await _unitOfWork.PostCategoryRepository.SoftDeleteAsync(postCategory);
                }
                else
                {
                    await _unitOfWork.PostCategoryRepository.DeleteAsync(postCategory);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<PostCategoryDTO?> GetPostCategoryByIdAsync(string postCategoryId)
        {
            return await _unitOfWork.PostCategoryRepository
                .Query()
                .Where(x => x.PostCategoryId.Equals(postCategoryId))
                .Select(x => new PostCategoryDTO
                {
                    PostCategoryId = x.PostCategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                })
                .SingleOrDefaultAsync();
        }
    }
}
