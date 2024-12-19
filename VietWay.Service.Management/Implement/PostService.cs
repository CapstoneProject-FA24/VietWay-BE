using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Cloudinary;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class PostService(IUnitOfWork unitOfWork, 
        ITimeZoneHelper timeZoneHelper,
        ICloudinaryService cloudinaryService,
        IIdGenerator idGenerator, IBackgroundJobClient backgroundJobClient) : IPostService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<PaginatedList<PostPreviewDTO>> GetAllPostAsync(
            string? nameSearch,
            List<string>? postCategoryIds,
            List<string>? provinceIds,
            List<PostStatus>? statuses,
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
            if (statuses?.Count > 0)
            {
                query = query.Where(x => statuses.Contains(x.Status));
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
            return new PaginatedList<PostPreviewDTO>
            {
                Items = items,
                Total = count,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<string> CreatePostAsync(Post post)
        {
            try
            {
                post.PostId ??= _idGenerator.GenerateId();
                post.CreatedAt = _timeZoneHelper.GetUTC7Now();
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
            Post? a = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId));
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                throw new ResourceNotFoundException("NOT_EXISTED_POST");
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

        public async Task UpdatePostAsync(Post newPost, string accountId)
        {
            Post? post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(newPost.PostId)) ??
                throw new ResourceNotFoundException("NOT_EXISTED_POST");

            bool isManagerUpdate = await _unitOfWork.AccountRepository.Query()
                .AnyAsync(x => x.AccountId.Equals(accountId) && x.Role == UserRole.Manager) && post.Status == PostStatus.Approved;

            bool isStaffUpdate = await _unitOfWork.AccountRepository.Query()
                .AnyAsync(x => x.AccountId.Equals(accountId) && x.Role == UserRole.Staff) && post.Status != PostStatus.Approved;

            if (!isStaffUpdate && !isManagerUpdate)
            {
                throw new InvalidActionException("INVALID_ACTION_UPDATE_POST");
            }

            post.Status = newPost.Status;
            post.Title = newPost.Title;
            post.Content = newPost.Content;
            post.PostCategoryId = newPost.PostCategoryId;
            post.Description = newPost.Description;
            post.ProvinceId = newPost.ProvinceId;
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
        public async Task<PostDetailDTO?> GetPostByIdAsync(string postId)
        {
            return await _unitOfWork.PostRepository
                .Query()
                .Where(x => x.PostId.Equals(postId))
                .Include(x => x.Province)
                .Include(x => x.PostCategory)
                .Select(x => new PostDetailDTO
                {
                    PostId = x.PostId,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    Content = x.Content,
                    CreateAt = x.CreatedAt,
                    PostCategoryId = x.PostCategoryId,
                    PostCategoryName = x.PostCategory.Name,
                    ProvinceId = x.ProvinceId,
                    ProvinceName = x.Province.Name,
                    Description = x.Description,
                    Status = x.Status,
                    XTweetId = x.XTweetId,
                    FacebookPostId = x.FacebookPostId
                })
                .SingleOrDefaultAsync();
        }

        public async Task ChangePostStatusAsync(string postId, string accountId, PostStatus postStatus, string? reason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Account? account = await _unitOfWork.AccountRepository.Query()
                    .SingleOrDefaultAsync(x => x.AccountId.Equals(accountId)) ??
                    throw new UnauthorizedException("UNAUTHORIZED");
                Post? post = await _unitOfWork.PostRepository.Query()
                    .SingleOrDefaultAsync(x => x.PostId.Equals(postId)) ??
                    throw new ResourceNotFoundException("NOT_EXISTED_POST");

                bool isManagerApproveOrDenyPendingPost = (PostStatus.Approved == postStatus || PostStatus.Rejected == postStatus) &&
                    UserRole.Manager == account.Role && PostStatus.Pending == post.Status;
                bool isStaffSubmitDraftPostForPreview = (PostStatus.Pending == postStatus) && UserRole.Staff == account.Role &&
                    PostStatus.Draft == post.Status;

                if (isStaffSubmitDraftPostForPreview)
                {
                    post.Status = PostStatus.Pending;
                }
                else if (isManagerApproveOrDenyPendingPost)
                {
                    post.Status = postStatus;
                }
                else
                {
                    throw new UnauthorizedException("UNAUTHORIZED");
                }

                await _unitOfWork.PostRepository.UpdateAsync(post);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdatePostImageAsync(string postId, IFormFile newImages)
        {
            Post post = await _unitOfWork.PostRepository.Query()
                .SingleOrDefaultAsync(x => x.PostId.Equals(postId))
                ?? throw new ResourceNotFoundException("NOT_EXISTED_POST");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (newImages != null)
                {
                    string imageId = $"{postId}-image-{_idGenerator.GenerateId()}";
                    using MemoryStream memoryStream = new();
                    using Stream stream = newImages.OpenReadStream();
                    await stream.CopyToAsync(memoryStream);
                    await _cloudinaryService.UploadImageAsync(imageId, newImages.FileName, memoryStream.ToArray());
                    post.ImageUrl = _cloudinaryService.GetImage(imageId);
                }

                await _unitOfWork.PostRepository.UpdateAsync(post);
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
