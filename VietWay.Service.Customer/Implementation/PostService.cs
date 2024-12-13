﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.CustomExceptions;

namespace VietWay.Service.Customer.Implementation
{
    public class PostService(IUnitOfWork unitOfWork) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<PaginatedList<PostPreviewDTO>> GetCustomerLikedPostPreviewsAsync(string customerId, int pageSize, int pageIndex)
        {
            IQueryable<Post> query = _unitOfWork.PostRepository.Query()
                .Where(x => x.PostLikes.Any(y => y.CustomerId.Equals(customerId)));
            int count = await query.CountAsync();
            List<PostPreviewDTO> items = await query
                .OrderBy(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PostPreviewDTO()
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    PostCategoryName = x.PostCategory.Name,
                    ProvinceName = x.Province.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    IsLiked = true
                }).ToListAsync();
            return new PaginatedList<PostPreviewDTO>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = count,
            };
        }

        public async Task<PostDetailDTO?> GetPostDetailAsync(string postId, string? customerId)
        {
            return await _unitOfWork.PostRepository.Query()
                .Select(x => new PostDetailDTO
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    Content = x.Content,
                    PostCategoryName = x.PostCategory.Name,
                    ProvinceName = x.Province.Name,
                    Description = x.Description,
                    PostCategoryId = x.PostCategoryId,
                    ProvinceId = x.ProvinceId,
                    CreatedAt = x.CreatedAt,
                    IsLiked = customerId != null && x.PostLikes.Any(y => y.CustomerId.Equals(customerId))
                })
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId));
        }

        public async Task<PaginatedList<PostPreviewDTO>> GetPostPreviewsAsync(string? nameSearch, List<string>? provinceIds, 
            List<string>? postCategoryIds,string? customerId, int pageSize, int pageIndex)
        {
            IQueryable<Post> query = _unitOfWork.PostRepository.Query()
                .Where(x => x.Status == PostStatus.Approved && x.IsDeleted == false);
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(x => x.Title.Contains(nameSearch));
            }
            if (provinceIds?.Count > 0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (postCategoryIds?.Count > 0)
            {
                query = query.Where(x => postCategoryIds.Contains(x.PostCategoryId));
            }
            int count = await query.CountAsync();
            List<PostPreviewDTO> items = await query
                .OrderBy(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new PostPreviewDTO()
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    PostCategoryName = x.PostCategory.Name,
                    ProvinceName = x.Province.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    IsLiked = customerId !=null && x.PostLikes.Any(y => y.CustomerId.Equals(customerId)),
                }).ToListAsync();
            return new PaginatedList<PostPreviewDTO>
            {
                Items = items,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = count,
            };
        }

        public async Task TogglePostLikeAsync(string postId, string? customerId, bool isLike)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                PostLike? postLike = await _unitOfWork.PostLikeRepository.Query()
                    .SingleOrDefaultAsync(x => x.PostId.Equals(postId) && x.CustomerId.Equals(customerId));
                if (null == postLike && isLike)
                {
                    await _unitOfWork.PostLikeRepository.CreateAsync(new PostLike
                    {
                        CustomerId = customerId,
                        PostId = postId,
                    });
                }
                else if (null != postLike && false == isLike)
                {
                    await _unitOfWork.PostLikeRepository.DeleteAsync(postLike);
                }
                else
                {
                    throw new InvalidActionException(nameof(PostLike));
                }
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
