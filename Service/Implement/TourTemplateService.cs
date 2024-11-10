using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class TourTemplateService(IUnitOfWork unitOfWork, IIdGenerator idGenerator,
        ICloudinaryService cloudinaryService, ITimeZoneHelper timeZoneHelper) : ITourTemplateService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<string> CreateTemplateAsync(TourTemplate tourTemplate)
        {
            tourTemplate.TourTemplateId = _idGenerator.GenerateId();
            tourTemplate.CreatedAt = DateTime.UtcNow;
#warning use utc+7 now
            foreach (var province in tourTemplate.TourTemplateProvinces ?? [])
            {
                province.TourTemplateId = tourTemplate.TourTemplateId;
            }
            foreach (var schedule in tourTemplate.TourTemplateSchedules ?? [])
            {
                schedule.TourTemplateId = tourTemplate.TourTemplateId;
                foreach (var attractionSchedule in schedule.AttractionSchedules ?? [])
                {
                    attractionSchedule.TourTemplateId = tourTemplate.TourTemplateId;
                }
            }
            await _unitOfWork.TourTemplateRepository.CreateAsync(tourTemplate);
            return tourTemplate.TourTemplateId;
        }
        public async Task DeleteTemplateAsync(TourTemplate tourTemplate)
        {
            await _unitOfWork.TourTemplateRepository.SoftDeleteAsync(tourTemplate);
        }

        public async Task<(int totalCount, List<TourTemplate> items)> GetAllTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            TourTemplateStatus? status,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.TourName.Contains(nameSearch));
            }
            if (templateCategoryIds != null && templateCategoryIds.Count > 0)
            {
                query = query.Where(x => templateCategoryIds.Contains(x.TourCategoryId));
            }
            if (provinceIds != null && provinceIds.Count > 0)
            {
                query = query.Where(x => x.TourTemplateProvinces.Select(x => x.ProvinceId).Any(p => provinceIds.Contains(p)));
            }
            if (durationIds != null && durationIds.Count > 0)
            {
                query = query.Where(x => durationIds.Contains(x.DurationId));
            }
            if (status != null)
            {
                query = query.Where(x => x.Status.Equals(status));
            }
            int count = await query.CountAsync();
            List<TourTemplate> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x => x.TourCategory)
                .Include(x => x.TourDuration)
                .ToListAsync();
            return (count, items);
        }

        public async Task<TourTemplate?> GetTemplateByIdAsync(string id)
        {
            return await _unitOfWork
                .TourTemplateRepository
                .Query()
                .Include(x => x.TourTemplateSchedules)
                .ThenInclude(x => x.AttractionSchedules)
                .ThenInclude(x => x.Attraction)
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x => x.TourDuration)
                .Include(x => x.TourCategory)
                .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(id));
        }

        public async Task UpdateTemplateAsync(TourTemplate tourTemplate, List<TourTemplateSchedule> newSchedule)
        {
            tourTemplate.TourTemplateSchedules = newSchedule;
            await _unitOfWork.TourTemplateRepository.UpdateAsync(tourTemplate);
        }

        public async Task UpdateTemplateImageAsync(TourTemplate tourTemplate, List<IFormFile>? imageFiles, List<string>? removedImageIds)
        {
            if (imageFiles != null)
            {
                foreach (var imageFile in imageFiles)
                {
                    using Stream stream = imageFile.OpenReadStream();
                    string imageId = _idGenerator.GenerateId();
                    TourTemplateImage attractionImage = new()
                    {
                        TourTemplateId = tourTemplate.TourTemplateId,
                        ImageId = imageId,
                        ImageUrl = _cloudinaryService.GetImage(imageId)
                    };
                    tourTemplate.TourTemplateImages.Add(attractionImage);
                }
            }
            if (removedImageIds?.Count > 0)
            {
                var removedImages = tourTemplate.TourTemplateImages
                    .Where(x => removedImageIds.Contains(x.ImageId))
                    .ToList();
                foreach (var image in removedImages)
                {
                    tourTemplate.TourTemplateImages.Remove(image);
                }
                //_ = await _cloudinaryService.DeleteImages(removedImages.Select(x => x.Image.PublicId));
                await _unitOfWork.TourTemplateRepository.UpdateAsync(tourTemplate);
            }
        }

        public async Task<(int totalCount, List<TourTemplate> items)> GetAllApprovedTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.TourName.Contains(nameSearch));
            }
            if (templateCategoryIds != null && templateCategoryIds.Count > 0)
            {
                query = query.Where(x => templateCategoryIds.Contains(x.TourCategoryId));
            }
            if (provinceIds != null && provinceIds.Count > 0)
            {
                query = query.Where(x => x.TourTemplateProvinces.Select(x => x.ProvinceId).Any(p => provinceIds.Contains(p)));
            }
            if (durationIds != null && durationIds.Count > 0)
            {
                query = query.Where(x => durationIds.Contains(x.DurationId));
            }
            int count = await query.CountAsync();
            List<TourTemplate> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Where(x => x.Status == TourTemplateStatus.Approved)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x => x.TourCategory)
                .Include(x => x.TourDuration)
                .ToListAsync();
            return (count, items);
        }
        public async Task<(int count, List<TourTemplateWithTourInfoDTO> items)> GetAllTemplateWithActiveToursAsync(
            string? nameSearch, List<string>? templateCategoryIds, List<string>? provinceIds, List<int>? numberOfDay, DateTime? startDateFrom,
            DateTime? startDateTo, decimal? minPrice, decimal? maxPrice, int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x => x.IsDeleted == false &&
                            x.Tours.Any(y => y.StartDate >= _timeZoneHelper.GetUTC7Now() &&
                                             y.Status == TourStatus.Opened));
            if (false == string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(x => x.TourName.Contains(nameSearch));
            }
            if (templateCategoryIds?.Count > 0)
            {
                query = query.Where(x => templateCategoryIds.Contains(x.TourCategoryId));
            }
            if (provinceIds?.Count > 0)
            {
                query = query.Where(x => x.TourTemplateProvinces.Any(y => provinceIds.Contains(y.ProvinceId)));
            }
            if (numberOfDay?.Count > 0)
            {
                query = query.Where(x => numberOfDay.Contains(x.TourDuration.NumberOfDay));
            }
            if (startDateFrom != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.StartDate >= startDateFrom));
            }
            if (startDateTo != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.StartDate <= startDateTo));
            }
            if (minPrice != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.DefaultTouristPrice >= minPrice));
            }
            if (maxPrice != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.DefaultTouristPrice <= maxPrice));
            }

            int count = await query.CountAsync();

            List<TourTemplateWithTourInfoDTO> items = await query
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                    .ThenInclude(x => x.Province)
                .Include(x => x.TourCategory)
                .Include(x => x.TourDuration)
                .Include(x => x.Tours)
                .OrderBy(x => x.TourCategoryId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TourTemplateWithTourInfoDTO
                {
                    TourTemplateId = x.TourTemplateId,
                    Code = x.Code,
                    Duration = x.TourDuration.DurationName,
                    TourCategory = x.TourCategory.Name,
                    ImageUrl = x.TourTemplateImages.FirstOrDefault().ImageUrl,
                    MinPrice = x.Tours.Where(x => x.Status == TourStatus.Opened).Select(y => (decimal)y.DefaultTouristPrice).Min(),
                    Provinces = x.TourTemplateProvinces.Select(y => y.Province.Name).ToList(),
                    StartDate = x.Tours.Where(x => x.Status == TourStatus.Opened).Select(y => (DateTime)y.StartDate).ToList(),
                    TourName = x.TourName,
                })
                .ToListAsync();
            return (count, items);
        }

        public async Task<List<TourTemplatePreviewDTO>> GetTourTemplatesPreviewRelatedToAttractionAsync(string attractionId, int previewCount)
        {
            return await _unitOfWork.TourTemplateRepository.Query()
                .Where(x => x.Tours.Any(t => t.StartDate >= _timeZoneHelper.GetUTC7Now() && t.Status == TourStatus.Opened))
                .Where(x => x.TourTemplateSchedules.Any(ts => ts.AttractionSchedules.Any(a => a.AttractionId.Equals(attractionId))))
                .Take(previewCount)
                .Include(x => x.TourDuration)
                .Include(x => x.TourTemplateImages)
                .Include(x => x.TourTemplateProvinces)
                    .ThenInclude(x => x.Province)
                .Include(x => x.TourCategory)
                .Select(x => new TourTemplatePreviewDTO
                {
                    Code = x.Code,
                    Duration = x.TourDuration.DurationName,
                    ImageUrl = x.TourTemplateImages.FirstOrDefault().ImageUrl,
                    Provinces = x.TourTemplateProvinces.Select(y => y.Province.Name).ToList(),
                    TourCategory = x.TourCategory.Name,
                    TourName = x.TourName,
                    TourTemplateId = x.TourTemplateId
                }).ToListAsync();
        }

        public async Task UpdateTourTemplateImageAsync(string tourTemplateId, string staffId, List<IFormFile>? newImages, List<string>? imageIdsToRemove)
        {
            TourTemplate tourTemplate = await _unitOfWork.TourTemplateRepository.Query()
                .Include(x => x.TourTemplateImages).SingleOrDefaultAsync(x => x.TourTemplateId.Equals(tourTemplateId))
                ?? throw new ResourceNotFoundException("Tour template not found");
            try
            {
                var enqueuedJobs = new List<Action>();
                await _unitOfWork.BeginTransactionAsync();
                tourTemplate.TourTemplateImages ??= [];
                if (newImages != null)
                {
                    foreach (var imageFile in newImages)
                    {
                        string imageId = _idGenerator.GenerateId();
                        using MemoryStream memoryStream = new();
                        using Stream stream = imageFile.OpenReadStream();
                        await stream.CopyToAsync(memoryStream);
                        enqueuedJobs.Add(() => _cloudinaryService.UploadImageAsync(imageId, imageFile.FileName, memoryStream.ToArray()));
                        tourTemplate.TourTemplateImages.Add(new TourTemplateImage
                        {
                            TourTemplateId = tourTemplate.TourTemplateId,
                            ImageId = imageId,
                            ImageUrl = _cloudinaryService.GetImage(imageId)
                        });
                    }
                }
                List<TourTemplateImage>? imagesToRemove = null;
                if (imageIdsToRemove?.Count > 0)
                {
                    imagesToRemove = tourTemplate.TourTemplateImages
                        .Where(x => imageIdsToRemove.Contains(x.ImageId))
                        .ToList();
                    foreach (TourTemplateImage image in imagesToRemove)
                    {
                        tourTemplate.TourTemplateImages.Remove(image);
                    }
                    enqueuedJobs.Add(() => _cloudinaryService.DeleteImagesAsync(imageIdsToRemove));
                }
                await _unitOfWork.TourTemplateRepository.UpdateAsync(tourTemplate);

                await _unitOfWork.CommitTransactionAsync();
                enqueuedJobs.ForEach(job => job.Invoke());
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task ChangeTourTemplateStatusAsync(string tourTemplateId, TourTemplateStatus tourTemplateStatus)
        {
            TourTemplate? tourTemplate = await _unitOfWork.TourTemplateRepository.Query()
                .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(tourTemplateId)) ??
                throw new ResourceNotFoundException("Tour Template not found");

            tourTemplate.Status = tourTemplateStatus;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourTemplateRepository.UpdateAsync(tourTemplate);
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
