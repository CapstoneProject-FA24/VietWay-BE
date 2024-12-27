using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.Cloudinary;
using VietWay.Service.ThirdParty.Twitter;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class AttractionService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IIdGenerator idGenerator,
        IBackgroundJobClient backgroundJobClient, ITimeZoneHelper timeZoneHelper) : IAttractionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;

        public async Task<string> CreateAttractionAsync(Attraction attraction)
        {
            try
            {
                attraction.AttractionId ??= _idGenerator.GenerateId();
                attraction.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.AttractionRepository.CreateAsync(attraction);
                await _unitOfWork.CommitTransactionAsync();
                return attraction.AttractionId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteAttractionAsync(string attractionId)
        {
            Attraction? attraction = await _unitOfWork.AttractionRepository.Query()
                .SingleOrDefaultAsync(x => x.AttractionId.Equals(attractionId) && !x.IsDeleted) ??
                throw new ResourceNotFoundException("NOT_EXIST_ATTRACTION");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.AttractionRepository.SoftDeleteAsync(attraction);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PaginatedList<AttractionPreviewDTO>> GetAllAttractionsWithCreatorAsync(
            string? nameSearch, List<string>? provinceIds, List<string>? attractionCategoryIds, List<AttractionStatus>? statuses,
            int pageSize, int pageIndex)
        {
            IQueryable<Attraction> query = _unitOfWork.AttractionRepository.Query();
            if (false == string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
            if (provinceIds?.Count > 0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (attractionCategoryIds?.Count > 0)
            {
                query = query.Where(x => attractionCategoryIds.Contains(x.AttractionCategoryId));
            }
            if (statuses?.Count > 0)
            {
                query = query.Where(x => statuses.Contains(x.Status));
            }
            int count = await query.CountAsync();
            List<AttractionPreviewDTO> attractions = await query
                .Where(x => !x.IsDeleted)
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .Include(x => x.AttractionCategory)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .Select(x => new AttractionPreviewDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = x.Province.Name,
                    AttractionType = x.AttractionCategory.Name,
                    Status = x.Status,
                    ImageUrl = x.AttractionImages.FirstOrDefault() != null ? x.AttractionImages.FirstOrDefault().ImageUrl : null,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
            return new PaginatedList<AttractionPreviewDTO>
            {
                Items = attractions,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = count
            };
        }

        public async Task<AttractionDetailDTO?> GetApprovedAttractionDetailById(string attractionId)
        {
            return await _unitOfWork.AttractionRepository.Query()
                .Where(x => x.AttractionId.Equals(attractionId) && x.Status == AttractionStatus.Approved && x.IsDeleted == false)
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .Include(x => x.AttractionCategory)
                .Select(x => new AttractionDetailDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = new ProvinceBriefPreviewDTO
                    {
                        ProvinceId = x.ProvinceId,
                        ProvinceName = x.Province.Name
                    },
                    AttractionType = new AttractionCategoryPreviewDTO
                    {
                        AttractionCategoryId = x.AttractionCategoryId,
                        Name = x.AttractionCategory.Name
                    },
                    Description = x.Description,
                    Images = x.AttractionImages.Select(x => new ImageDTO
                    {
                        ImageId = x.ImageId,
                        ImageUrl = x.ImageUrl
                    }).ToList(),
                    ContactInfo = x.ContactInfo,
                    GooglePlaceId = x.GooglePlaceId,
                    Website = x.Website,
                    Status = x.Status,
                    CreatedDate = x.CreatedAt
                }).SingleOrDefaultAsync();
        }

        public async Task<AttractionDetailDTO?> GetAttractionWithCreateDateByIdAsync(string attractionId)
        {
            return await _unitOfWork.AttractionRepository
                .Query()
                .Where(x => x.AttractionId.Equals(attractionId) && !x.IsDeleted)
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .Include(x => x.AttractionCategory)
                .Select(x => new AttractionDetailDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = new ProvinceBriefPreviewDTO
                    {
                        ProvinceId = x.ProvinceId,
                        ProvinceName = x.Province.Name
                    },
                    AttractionType = new AttractionCategoryPreviewDTO
                    {
                        AttractionCategoryId = x.AttractionCategoryId,
                        Name = x.AttractionCategory.Name
                    },
                    Status = x.Status,
                    CreatedDate = x.CreatedAt,
                    Description = x.Description,
                    Images = x.AttractionImages.Select(x => new ImageDTO
                    {
                        ImageId = x.ImageId,
                        ImageUrl = x.ImageUrl
                    }).ToList(),
                    ContactInfo = x.ContactInfo,
                    GooglePlaceId = x.GooglePlaceId,
                    Website = x.Website
                })
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAttractionAsync(Attraction newAttraction, string accountId)
        {
            Attraction? attraction = await _unitOfWork.AttractionRepository.Query()
                .SingleOrDefaultAsync(x => x.AttractionId.Equals(newAttraction.AttractionId) && !x.IsDeleted) ??
                throw new ResourceNotFoundException("NOT_EXIST_ATTRACTION");

            bool isManagerUpdate = await _unitOfWork.AccountRepository.Query()
                .AnyAsync(x => x.AccountId.Equals(accountId) && x.Role == UserRole.Manager) && attraction.Status == AttractionStatus.Approved;

            bool isStaffUpdate = await _unitOfWork.AccountRepository.Query()
                .AnyAsync(x => x.AccountId.Equals(accountId) && x.Role == UserRole.Staff) && attraction.Status != AttractionStatus.Approved;

            if (!isStaffUpdate && !isManagerUpdate)
            {
                throw new InvalidActionException("INVALID_ACTION_UPDATE_ATTRACTION");
            }

            attraction.Status = newAttraction.Status;
            attraction.Name = newAttraction.Name;
            attraction.Address = newAttraction.Address;
            attraction.ContactInfo = newAttraction.ContactInfo;
            attraction.Description = newAttraction.Description;
            attraction.ProvinceId = newAttraction.ProvinceId;
            attraction.AttractionCategoryId = newAttraction.AttractionCategoryId;
            attraction.GooglePlaceId = newAttraction.GooglePlaceId;
            attraction.Website = newAttraction.Website;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.AttractionRepository.UpdateAsync(attraction);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task UpdateAttractionImageAsync(string attractionId, List<IFormFile>? imageFiles,
            List<string>? imageIdsToRemove)
        {
            Attraction attraction = await _unitOfWork.AttractionRepository.Query()
                .Include(x => x.AttractionImages).SingleOrDefaultAsync(x => x.AttractionId.Equals(attractionId) && !x.IsDeleted)
                ?? throw new ResourceNotFoundException("NOT_EXIST_ATTRACTION");
            try
            {
                var enqueuedJobs = new List<Action>();
                await _unitOfWork.BeginTransactionAsync();
                attraction.AttractionImages ??= [];
                if (imageFiles != null)
                {
                    foreach (var imageFile in imageFiles)
                    {
                        string imageId = _idGenerator.GenerateId();
                        using Stream stream = imageFile.OpenReadStream();
                        using MemoryStream memoryStream = new();
                        await stream.CopyToAsync(memoryStream);
                        enqueuedJobs.Add(async () => await _cloudinaryService.UploadImageAsync(imageId, imageFile.FileName, memoryStream.ToArray()));
                        attraction.AttractionImages.Add(new AttractionImage
                        {
                            AttractionId = attraction.AttractionId,
                            ImageId = imageId,
                            ImageUrl = _cloudinaryService.GetImage(imageId)
                        });
                    }
                }
                List<AttractionImage>? imagesToRemove = null;
                if (imageIdsToRemove?.Count > 0)
                {
                    imagesToRemove = attraction.AttractionImages
                        .Where(x => imageIdsToRemove.Contains(x.ImageId))
                        .ToList();
                    foreach (AttractionImage image in imagesToRemove)
                    {
                        attraction.AttractionImages.Remove(image);
                    }
                    enqueuedJobs.Add(async () => await _cloudinaryService.DeleteImagesAsync(imageIdsToRemove));
                }
                await _unitOfWork.AttractionRepository.UpdateAsync(attraction);

                await _unitOfWork.CommitTransactionAsync();
                enqueuedJobs.ForEach(job => job.Invoke());
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateAttractionStatusAsync(string attractionId, string accountId, AttractionStatus status, string? reason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Account? account = await _unitOfWork.AccountRepository.Query()
                    .SingleOrDefaultAsync(x => x.AccountId.Equals(accountId) && !x.IsDeleted) ??
                    throw new UnauthorizedException("UNAUTHORIZED");
                Attraction? attraction = await _unitOfWork.AttractionRepository.Query()
                    .SingleOrDefaultAsync(x => attractionId.Equals(x.AttractionId)) ??
                    throw new ResourceNotFoundException("NOT_EXIST_ATTRACTION");

                bool isManagerApproveOrDenyPendingAttraction = (AttractionStatus.Approved == status || AttractionStatus.Rejected == status) &&
                    UserRole.Manager == account.Role && AttractionStatus.Pending == attraction.Status;
                bool isStaffSubmitDraftAttractionForPreview = AttractionStatus.Pending == status && UserRole.Staff == account.Role &&
                    AttractionStatus.Draft == attraction.Status;

                if (isStaffSubmitDraftAttractionForPreview)
                {
                    attraction.Status = AttractionStatus.Pending;
                }
                else if (isManagerApproveOrDenyPendingAttraction)
                {
                    attraction.Status = status;
                }
                else
                {
                    throw new UnauthorizedException("UNAUTHORIZED");
                }

                await _unitOfWork.AttractionRepository.UpdateAsync(attraction);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PaginatedList<AttractionPreviewDTO>> GetAllApproveAttractionsAsync(
            string? nameSearch, List<string>? provinceIds, List<string>? attractionCategoryIds, List<string>? attractionIds,
            int pageSize, int pageIndex)
        {
            IQueryable<Attraction> query = _unitOfWork.AttractionRepository.Query();
            if (false == string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
            if (provinceIds?.Count > 0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (attractionCategoryIds?.Count > 0)
            {
                query = query.Where(x => attractionCategoryIds.Contains(x.AttractionCategoryId));
            }
            if (attractionIds?.Count > 0)
            {
                query = query.Where(x => !attractionIds.Contains(x.AttractionId));
            }
            int count = await query.CountAsync();
            List<AttractionPreviewDTO> attractions = await query
                .Where(x => !x.IsDeleted)
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .Include(x => x.AttractionCategory)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .Select(x => new AttractionPreviewDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = x.Province.Name,
                    AttractionType = x.AttractionCategory.Name,
                    Status = x.Status,
                    ImageUrl = x.AttractionImages.FirstOrDefault() != null ? x.AttractionImages.FirstOrDefault().ImageUrl : null,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
            return new PaginatedList<AttractionPreviewDTO>
            {
                Items = attractions,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = count
            };
        }
    }
}
