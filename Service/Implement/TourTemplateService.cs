using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;
using VietWay.Service.ThirdParty;
using VietWay.Util.DateTimeHelper;
using VietWay.Util.IdHelper;

namespace VietWay.Service.Implement
{
    public class TourTemplateService(
        IUnitOfWork unitOfWork, 
        IIdGenerator idGenerator, 
        ICloudinaryService cloudinaryService,
        ITimeZoneHelper timeZoneHelper) : ITourTemplateService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<string> CreateTemplateAsync(TourTemplate tourTemplate)
        {
            tourTemplate.TourTemplateId = _idGenerator.GenerateId();
            tourTemplate.CreatedDate = DateTime.UtcNow;
            foreach(var province in tourTemplate.TourTemplateProvinces ?? [])
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
            await _unitOfWork.TourTemplateRepository.Create(tourTemplate);
            return tourTemplate.TourTemplateId;
        }
        public async Task DeleteTemplateAsync(TourTemplate tourTemplate)
        {
            await _unitOfWork.TourTemplateRepository.SoftDelete(tourTemplate);
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
                .Where(x=>x.IsDeleted == false);
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
                query = query.Where(x=>x.TourTemplateProvinces.Select(x=>x.ProvinceId).Any(p=>provinceIds.Contains(p)));
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
                .OrderByDescending(x => x.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplateImages)
                .ThenInclude(x => x.Image)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x=>x.TourCategory)
                .Include(x=>x.TourDuration)
                .Include(x => x.Creator)
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
                .ThenInclude(x => x.Image)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x=>x.TourDuration)
                .Include(x=>x.TourCategory)
                .Include(x=>x.Creator)
                .SingleOrDefaultAsync(x => x.TourTemplateId.Equals(id));
        }

        public async Task UpdateTemplateAsync(TourTemplate tourTemplate, List<TourTemplateSchedule> newSchedule)
        {
            IEnumerable<AttractionSchedule> deleteAttractionSchedule = tourTemplate.TourTemplateSchedules.SelectMany(x => x.AttractionSchedules);
            await _unitOfWork.AttractionScheduleRepository.DeleteRange(deleteAttractionSchedule);
            tourTemplate.TourTemplateSchedules = newSchedule;
            await _unitOfWork.TourTemplateRepository.Update(tourTemplate);
        }

        public async Task UpdateTemplateImageAsync(TourTemplate tourTemplate, List<IFormFile>? imageFiles, List<string>? removedImageIds)
        {
            if (imageFiles != null)
            {
                foreach (var imageFile in imageFiles)
                {
                    using Stream stream = imageFile.OpenReadStream();
                    var (publicId, secureUrl) = await _cloudinaryService.UploadImageAsync(stream, imageFile.FileName);
                    Image image = new()
                    {
                        ImageId = _idGenerator.GenerateId(),
                        PublicId = publicId,
                        Url = secureUrl,
                        ContentType = imageFile.ContentType,
                        FileName = imageFile.FileName
                    };
                    TourTemplateImage attractionImage = new()
                    {
                        TourTemplateId = tourTemplate.TourTemplateId,
                        ImageId = image.ImageId,
                        Image = image
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
                _ = await _cloudinaryService.DeleteImages(removedImages.Select(x => x.Image.PublicId));
                await _unitOfWork.TourTemplateRepository.Update(tourTemplate);
                await _unitOfWork.ImageRepository.DeleteRange(removedImages.Select(x => x.Image));
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
                .OrderByDescending(x => x.CreatedDate)
                .Where(x => x.Status == TourTemplateStatus.Approved)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplateImages)
                .ThenInclude(x => x.Image)
                .Include(x => x.TourTemplateProvinces)
                .ThenInclude(x => x.Province)
                .Include(x => x.TourCategory)
                .Include(x => x.TourDuration)
                .ToListAsync();
            return (count, items);
        }
        public async Task<(int count, List<TourTemplateWithTourInfoDTO> items)> GetAllTemplateWithActiveToursAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<int>? numberOfDay,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            decimal? minPrice,
            decimal? maxPrice,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x => x.IsDeleted == false && 
                            x.Tours.Any(y => y.StartDate >= _timeZoneHelper.GetUTC7Now() && 
                                             y.Status == TourStatus.Scheduled));
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
                query = query.Where(x=>x.TourTemplateProvinces.Any(y=>provinceIds.Contains(y.ProvinceId)));
            }
            if (numberOfDay?.Count > 0)
            {
                query = query.Where(x => numberOfDay.Contains(x.TourDuration.NumberOfDay));
            }
            if (startDateFrom != null)
            {
                query = query.Where(x=>x.Tours.Any(x=>x.StartDate >= startDateFrom));
            }
            if (startDateTo != null)
            {
                query = query.Where(x=>x.Tours.Any(x=>x.StartDate <= startDateTo));
            }
            if (minPrice != null)
            {
                query = query.Where(x => x.Tours.Any(x => x.Price >= minPrice));
            }
            if (maxPrice != null)
            {
                query = query.Where(x=>x.Tours.Any(x=>x.Price <= maxPrice));
            }

            int count = await query.CountAsync();

            List<TourTemplateWithTourInfoDTO> items = await query
                .Include(x => x.TourTemplateImages)
                    .ThenInclude(x => x.Image)
                .Include(x => x.TourTemplateProvinces)
                    .ThenInclude(x => x.Province)
                .Include(x => x.TourCategory)
                .Include(x => x.TourDuration)
                .Include(x => x.Tours)
                .OrderBy(x=>x.TourCategoryId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TourTemplateWithTourInfoDTO
                {
                    TourTemplateId = x.TourTemplateId,
                    Code = x.Code,
                    Duration = x.TourDuration.DurationName,
                    TourCategory = x.TourCategory.Name,
                    ImageUrl = x.TourTemplateImages.FirstOrDefault().Image.Url,
                    MinPrice = x.Tours.Where(x => x.Status == TourStatus.Scheduled).Select(y => y.Price).Min(),
                    Provinces = x.TourTemplateProvinces.Select(y => y.Province.ProvinceName).ToList(),
                    StartDate = x.Tours.Where(x=>x.Status == TourStatus.Scheduled).Select(y => y.StartDate).ToList(),
                    TourName = x.TourName,
                    Status = x.Status
                })
                .ToListAsync();
            return (count,items);
        }
    }
}
