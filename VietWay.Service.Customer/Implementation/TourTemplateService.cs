using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class TourTemplateService(IUnitOfWork unitOfWork, ITimeZoneHelper timeZoneHelper) : ITourTemplateService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<PaginatedList<TourTemplateWithTourInfoDTO>> GetTourTemplatesWithActiveToursAsync(string? nameSearch, 
            List<string>? templateCategoryIds,  List<string>? provinceIds, List<int>? numberOfDay, DateTime? startDateFrom, DateTime? startDateTo, 
            decimal? minPrice, decimal? maxPrice, int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .TourTemplateRepository
                .Query()
                .Where(x => x.IsDeleted == false && TourTemplateStatus.Approved == x.Status &&
                            x.Tours.Any(y => y.Status == TourStatus.Opened));
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
                .OrderBy(x => x.TourCategoryId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TourTemplateWithTourInfoDTO
                {
                    TourTemplateId = x.TourTemplateId,
                    Code = x.Code,
                    Duration = x.TourDuration.DurationName,
                    TourCategory = x.TourCategory.Name,
                    ImageUrl = x.TourTemplateImages.Select(x=>x.ImageUrl).FirstOrDefault() ?? "" ,
                    MinPrice = x.Tours.Where(x => x.Status == TourStatus.Opened && (false == minPrice.HasValue || minPrice <= x.DefaultTouristPrice))
                                    .Select(y => (decimal)y.DefaultTouristPrice).Min(),
                    Provinces = x.TourTemplateProvinces.Select(y => y.Province.Name).ToList(),
                    StartDate = x.Tours.Where(x => x.Status == TourStatus.Opened).Select(y => (DateTime)y.StartDate).ToList(),
                    TourName = x.TourName,
                    AverageRating = x.Tours.SelectMany(x => x.TourBookings).Select(x => x.TourReview).Average(x => x.Rating)
                })
                .ToListAsync();
            return new PaginatedList<TourTemplateWithTourInfoDTO>
            {
                Items = items,
                Total = count,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<TourTemplateDetailDTO?> GetTemplateByIdAsync(string tourTemplateId)
        {
            return await _unitOfWork.TourTemplateRepository.Query()
                .Where(x => x.TourTemplateId == tourTemplateId && false == x.IsDeleted && TourTemplateStatus.Approved == x.Status)
                .Select(x => new TourTemplateDetailDTO()
                {
                    Code = x.Code,
                    Description = x.Description,
                    Duration = new TourDurationDTO()
                    {
                        DurationName = x.TourDuration.DurationName,
                        NumberOfDay = x.TourDuration.NumberOfDay,
                        DurationId = x.DurationId
                    },
                    Images = x.TourTemplateImages.Select(y => new ImageDTO()
                    {
                        ImageId = y.ImageId,
                        Url = y.ImageUrl
                    }).ToList(),
                    Note = x.Note,
                    Provinces = x.TourTemplateProvinces.Select(y => new ProvincePreviewDTO()
                    {
                        ProvinceId = y.ProvinceId,
                        Name = y.Province.Name
                    }).ToList(),
                    Schedules = x.TourTemplateSchedules.Select(y => new ScheduleDTO()
                    {
                        Attractions = y.AttractionSchedules.Select(z => new AttractionPreviewDTO()
                        {
                            AttractionId = z.AttractionId,
                            Name = z.Attraction!.Name,
                            ImageUrl = z.Attraction!.AttractionImages!.Select(x => x.ImageUrl).FirstOrDefault(),
                            Address = z.Attraction.Address,
                            AttractionCategory = z.Attraction!.AttractionCategory!.Name,
                            Province = z.Attraction!.Province!.Name,
                            AverageRating = z.Attraction!.AttractionReviews!.Where(r => false == r.IsDeleted).Average(r => r.Rating),
                        }).ToList(),
                        Description = y.Description,
                        Title = y.Title,
                        DayNumber = y.DayNumber
                    }).ToList(),
                    TourCategory = new TourCategoryDTO()
                    {
                        Name = x.TourCategory.Name,
                        TourCategoryId = x.TourCategoryId,
                        Description = x.Description
                    },
                    TourName = x.TourName,
                    TourTemplateId = x.TourTemplateId,
                    AverageRating = x.Tours.SelectMany(x => x.TourBookings).Select(x => x.TourReview).Average(x => x.Rating),
                    RatingCount = x.Tours.SelectMany(x=>x.TourBookings)
                        .Select(x=>x.TourReview)
                        .GroupBy(x => x.Rating)
                        .Select(x => new RatingDTO()
                        {
                            Rating = x.Key,
                            Count = x.Count()
                        }).ToList()
                }).SingleOrDefaultAsync();
        }
        public Task<List<TourTemplatePreviewDTO>> GetTourTemplatePreviewsByAttractionId(string attractionId, int previewCount)
        {
            return _unitOfWork.TourTemplateRepository.Query()
                .Where(x => x.IsDeleted == false && TourTemplateStatus.Approved == x.Status &&
                            x.Tours.Any(y => y.Status == TourStatus.Opened) &&
                            x.TourTemplateSchedules.Any(y => y.AttractionSchedules.Any(z => z.AttractionId == attractionId)))
                .Select(x => new TourTemplatePreviewDTO()
                {
                    Code = x.Code,
                    Duration = x.TourDuration.DurationName,
                    ImageUrl = x.TourTemplateImages.FirstOrDefault().ImageUrl,
                    TourName = x.TourName,
                    TourTemplateId = x.TourTemplateId,
                    Provinces = x.TourTemplateProvinces.Select(y => y.Province.Name).ToList(),
                    TourCategory = x.TourCategory.Name,
                    Price = x.Tours.Where(y => y.Status == TourStatus.Opened).Select(y => (decimal)y.DefaultTouristPrice).Min(),
                    AverageRating = x.Tours.SelectMany(y => y.TourBookings).Select(y => y.TourReview).Average(y => y.Rating)
                }).Take(previewCount)
                .ToListAsync();
        }
    }
}
