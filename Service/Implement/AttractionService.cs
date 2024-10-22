﻿using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;
using VietWay.Service.Jobs;
using VietWay.Service.ThirdParty;
using VietWay.Util.CustomExceptions;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Implement
{
    public class AttractionService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IIdGenerator idGenerator,
        IBackgroundJobClient backgroundJobClient) : IAttractionService
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        public readonly IIdGenerator _idGenerator = idGenerator;
        public readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        public async Task<string> CreateAttractionAsync(Attraction attraction)
        {
            try
            {
                attraction.AttractionId ??= _idGenerator.GenerateId();
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
                .SingleOrDefaultAsync(x => x.AttractionId.Equals(attractionId)) ??
                throw new ResourceNotFoundException("Attraction not found");
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

        public async Task<(int totalCount, List<AttractionPreviewDTO> items)> GetAllApprovedAttractionsAsync(string? nameSearch, List<string>? provinceIds, List<string>? attractionTypeIds, int pageSize, int pageIndex)
        {
            IQueryable<Attraction> query = _unitOfWork.AttractionRepository.Query()
                .Where(x=>x.Status==AttractionStatus.Approved && x.IsDeleted == false);
            if (false == string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
            if (null != provinceIds && provinceIds.Count > 0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (null != attractionTypeIds && attractionTypeIds.Count > 0)
            {
                query = query.Where(x => attractionTypeIds.Contains(x.AttractionCategoryId));
            }
            int count = await query.CountAsync();
            List<AttractionPreviewDTO> items = await query
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .Include(x => x.AttractionCategory)
                .Select(x => new AttractionPreviewDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = x.Province.ProvinceName,
                    AttractionType = x.AttractionCategory.Name,
                    ImageUrl = x.AttractionImages.FirstOrDefault() != null ? x.AttractionImages.FirstOrDefault().ImageUrl : null
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (count, items);
        }

        public async Task<(int totalCount, List<AttractionPreviewWithCreateAtDTO> items)> GetAllAttractionsWithCreatorAsync(
            string? nameSearch, List<string>? provinceIds, List<string>? attractionCategoryIds, AttractionStatus? status, 
            int pageSize, int pageIndex)
        {
            IQueryable<Attraction> query = _unitOfWork.AttractionRepository.Query();
            if (false == string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
            if (null != provinceIds && provinceIds.Count > 0)
            {
                query = query.Where(x => provinceIds.Contains(x.ProvinceId));
            }
            if (null != attractionCategoryIds && attractionCategoryIds.Count > 0)
            {
                query = query.Where(x => attractionCategoryIds.Contains(x.AttractionCategoryId));
            }
            if (null != status)
            {
                query = query.Where(x => x.Status == status);
            }
            int count = await query.CountAsync();
            List<AttractionPreviewWithCreateAtDTO> attractions = await query
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .Include(x => x.AttractionCategory)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .Select(x => new AttractionPreviewWithCreateAtDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = x.Province.ProvinceName,
                    AttractionType = x.AttractionCategory.Name,
                    Status = x.Status,
                    ImageUrl = x.AttractionImages.FirstOrDefault() != null ? x.AttractionImages.FirstOrDefault().ImageUrl : null
                })
                .ToListAsync();
            return (count, attractions);
        }

        public async Task<AttractionDetailDTO?> GetApprovedAttractionDetailById(string attractionId)
        {
            return await _unitOfWork.AttractionRepository.Query()
                .Where(x=>x.AttractionId.Equals(attractionId) && x.Status == AttractionStatus.Approved && x.IsDeleted == false)
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .Include(x => x.AttractionCategory)
                .Select(x => new AttractionDetailDTO
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = new ProvinceBriefPreviewDTO { ProvinceId = x.ProvinceId, ProvinceName = x.Province.ProvinceName },
                    AttractionType = new AttractionCategoryPreviewDTO { AttractionCategoryId = x.AttractionCategoryId, Name = x.AttractionCategory.Name },
                    Description = x.Description,
                    Images = x.AttractionImages.Select(x => new ImageDTO() { ImageId = x.ImageId, Url = x.ImageUrl }).ToList(),
                    ContactInfo = x.ContactInfo,
                    GooglePlaceId = x.GooglePlaceId,
                    Website = x.Website
                }).SingleOrDefaultAsync();
        }

        public async Task<AttractionDetailWithCreatorDTO_NEEDFIX?> GetAttractionWithCreateDateByIdAsync(string attractionId)
        {
            return await _unitOfWork.AttractionRepository
                .Query()
                .Where(x => x.AttractionId.Equals(attractionId))
                .Include(x => x.AttractionImages)
                .Include(x => x.Province)
                .Include(x => x.AttractionCategory)
                .Select(x => new AttractionDetailWithCreatorDTO_NEEDFIX
                {
                    AttractionId = x.AttractionId,
                    Name = x.Name,
                    Address = x.Address,
                    Province = new ProvinceBriefPreviewDTO { ProvinceId = x.ProvinceId, ProvinceName = x.Province.ProvinceName },
                    AttractionType = new AttractionCategoryPreviewDTO { AttractionCategoryId = x.AttractionCategoryId, Name = x.AttractionCategory.Name },
                    Status = x.Status,
                    CreatedDate = x.CreatedAt,
                    Description = x.Description,
                    Images = x.AttractionImages.Select(x => new ImageDTO() { ImageId = x.ImageId, Url = x.ImageUrl }).ToList(),
                    ContactInfo = x.ContactInfo,
                    GooglePlaceId = x.GooglePlaceId,
                    Website = x.Website
                })
                .SingleOrDefaultAsync();
        }

        public async Task UpdateAttractionAsync(Attraction newAttraction)
        {
            Attraction? attraction = await _unitOfWork.AttractionRepository.Query()
                .SingleOrDefaultAsync(x => x.AttractionId.Equals(newAttraction.AttractionId)) ??
                throw new ResourceNotFoundException("Attraction not found");

            attraction.Status = AttractionStatus.Pending;
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
                .Include(x => x.AttractionImages).SingleOrDefaultAsync(x => x.AttractionId.Equals(attractionId))
                ?? throw new ResourceNotFoundException("Attraction not found");
            try
            {
                var enqueuedJobs = new List<Action>();
                await _unitOfWork.BeginTransactionAsync();
                attraction.AttractionImages ??= [] ;
                if (imageFiles != null)
                {
                    foreach (var imageFile in imageFiles)
                    {
                        string imageId = _idGenerator.GenerateId();
                        string filePath = Path.GetTempFileName();
                        using FileStream stream = new(filePath, FileMode.Create);
                        await imageFile.CopyToAsync(stream);
                        enqueuedJobs.Add(() => _backgroundJobClient.Enqueue<CloudImageProcessingJob>(
                            x => x.UploadImageAsync(imageId, filePath, imageFile.FileName)));
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
                    enqueuedJobs.Add(() => _backgroundJobClient.Enqueue<CloudImageProcessingJob>(
                        x => x.DeleteImagesAsync(imagesToRemove.Select(x=>x.ImageId))));
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
    }
}
