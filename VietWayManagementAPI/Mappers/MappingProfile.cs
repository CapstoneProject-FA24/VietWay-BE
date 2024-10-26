using AutoMapper;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.DataTransferObject;

namespace VietWay.API.Management.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TourTemplate, TourTemplateDetail>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => new DurationDetail()
                {
                    DayNumber = src.TourDuration.NumberOfDay,
                    DurationId = src.TourDuration.DurationId,
                    DurationName = src.TourDuration.DurationName
                }))
                .ForMember(dest => dest.TourCategory, opt => opt.MapFrom(src => new TourCategoryPreview()
                {
                    TourCategoryId = src.TourCategory.TourCategoryId,
                    TourCategoryName = src.TourCategory.Name
                }))
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.TourTemplateProvinces.Select(x => new ProvinceBriefPreviewDTO()
                {
                    ProvinceId = x.Province.ProvinceId,
                    ProvinceName = x.Province.ProvinceName
                }).ToList()))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.TourTemplateSchedules.Select(x => new ScheduleDetail
                {
                    DayNumber = x.DayNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Attractions = x.AttractionSchedules.Select(y => new AttractionBriefPreview()
                    {
                        AttractionId = y.AttractionId,
                        Name = y.Attraction.Name
                    }).ToList()
                }).ToList()))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.TourTemplateImages.Select(x => new ImageDTO()
                {
                    ImageId = x.ImageId,
                    Url = x.ImageUrl
                }).ToList()));
            CreateMap<TourTemplate, TourTemplatePreview>()
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.TourTemplateProvinces.Select(x => x.Province.ProvinceName).ToList()))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.TourTemplateImages.Select(x => x.ImageUrl).FirstOrDefault()))
                .ForMember(dest => dest.TourTemplateId, opt => opt.MapFrom(src => src.TourTemplateId))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.TourDuration.DurationName))
                .ForMember(dest => dest.TourCategory, opt => opt.MapFrom(src => src.TourCategory.Name));
            CreateMap<CreateAttractionRequest, Attraction>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? ""))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? ""))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? ""))
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo ?? ""))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsDraft ? AttractionStatus.Draft : AttractionStatus.Pending));
            CreateMap<TourCategory, TourCategoryPreview>()
                .ForMember(dest => dest.TourCategoryName, opt => opt.MapFrom(src => src.Name));
            CreateMap<TourDuration, DurationDetail>()
                .ForMember(dest => dest.DayNumber, opt => opt.MapFrom(src => src.NumberOfDay));
            CreateMap<AttractionCategory, AttractionCategoryPreviewDTO>();
            CreateMap<Tour, TourPreview>();
            CreateMap<CreateTourTemplateRequest, TourTemplate>()
                .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.TourName ?? ""))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code ?? ""))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? ""))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note ?? ""))
                .ForMember(dest => dest.TourTemplateProvinces, opt => opt.MapFrom(src => (src.ProvinceIds ?? new())
                    .Select(x => new TourTemplateProvince()
                    {
                        ProvinceId = x,
                        TourTemplateId = ""
                    })
                    .ToList()))
                .ForMember(dest => dest.TourTemplateSchedules, opt => opt.MapFrom(src => (src.Schedules ?? new())
                    .Select(x => new TourTemplateSchedule()
                    {
                        TourTemplateId = "",
                        DayNumber = x.DayNumber,
                        Title = x.Title ?? "",
                        Description = x.Description ?? "",
                        AttractionSchedules = (x.AttractionIds ?? new()).Select(y => new AttractionSchedule()
                        {
                            AttractionId = y,
                            TourTemplateId = "",
                            DayNumber = x.DayNumber
                        }).ToList()
                    }).ToList()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsDraft ? TourTemplateStatus.Draft : TourTemplateStatus.Pending));
            CreateMap<Tour, TourDetail>();
            CreateMap<AttractionSchedule, AttractionSchedulePreview>();
            CreateMap<TourReview, CustomerFeedbackPreview>();
            CreateMap<VnPayIPNRequest, VnPayIPN>();
        }
    }
}
