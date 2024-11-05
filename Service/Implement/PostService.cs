using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;
using VietWay.Service.ThirdParty.Cloudinary;
using VietWay.Util.CustomExceptions;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Implement
{
    public class PostService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService,
        IIdGenerator idGenerator, IBackgroundJobClient backgroundJobClient) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        public async Task<(int totalCount, List<PostPreviewDTO> items)> GetAllPostAsync(
            string? nameSearch,
            List<string>? postCategoryIds,
            List<string>? provinceIds,
            PostStatus? status,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .PostRepository
                .Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Title.Contains(nameSearch));
            }
            if (postCategoryIds != null && postCategoryIds.Count > 0)
            {
                query = query.Where(x => postCategoryIds.Contains(x.PostCategoryId));
            }
            if (provinceIds != null && provinceIds.Count > 0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status.Equals(status));
            }
            int count = await query.CountAsync();
            List<PostPreviewDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Include(x => x.Province)
                .Include(x => x.PostCategory)
                .Select(x => new PostPreviewDTO
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    Content = x.Content,
                    PostCategory = x.PostCategory.Name,
                    Province = x.Province.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    Status = x.Status
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (count, items);
        }

        public async Task<string> CreatePostAsync(Post post)
        {
            post.CreatedAt = DateTime.Now;
            try
            {
                post.PostId ??= _idGenerator.GenerateId();
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.PostRepository.CreateAsync(post);
                await _unitOfWork.CommitTransactionAsync();
                return post.PostId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeletePostAsync(string postId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("Post not found");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.PostRepository.SoftDeleteAsync(post);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdatePostAsync(Post newPost)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(newPost.PostId)) ??
                throw new ResourceNotFoundException("Post not found");

            post.Status = newPost.Status;
            post.Title = newPost.Title;
            post.Content = newPost.Content;
            post.PostCategoryId = newPost.PostCategoryId;
            post.Description = newPost.Description;
            post.ProvinceId = newPost.ProvinceId;
            post.ImageUrl = newPost.ImageUrl;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.PostRepository.UpdateAsync(post);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<PostPreviewDTO?> GetPostByIdAsync(string postId)
        {
            return await _unitOfWork.PostRepository
                .Query()
                .Where(x => x.PostId.Equals(postId))
                .Include(x => x.Province)
                .Include(x => x.PostCategory)
                .Select(x => new PostPreviewDTO
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    Content = x.Content,
                    PostCategory = x.PostCategory.Name,
                    Province = x.Province.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    Status = x.Status
                })
                .SingleOrDefaultAsync();
        }
    }
}
