using AutoMapper;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;

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
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.FullName))
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.TourTemplateProvinces.Select(x => new ProvincePreview()
                {
                    ProvinceId = x.Province.ProvinceId,
                    ProvinceName = x.Province.ProvinceName
                }).ToList()))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.TourTemplateSchedules.Select(x => new ScheduleDetail
                {
                    DayNumber = x.DayNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Attractions = x.AttractionSchedules.Select(y => new AttractionPreview()
                    {
                        AttractionId = y.AttractionId,
                        Name = y.Attraction.Name
                    }).ToList()
                }).ToList()))
                .ForMember(dest=>dest.Images, opt => opt.MapFrom(src=>src.TourTemplateImages.Select(x=> new ImageDetail() 
                { 
                    ImageId = x.ImageId, 
                    Url = x.Image.Url
                }).ToList()));
            CreateMap<TourTemplate, TourTemplatePreview>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.FullName))
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.TourTemplateProvinces.Select(x => x.Province.ProvinceName).ToList()))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.TourTemplateImages.Select(x => x.Image.Url).FirstOrDefault()))
                .ForMember(dest => dest.TourTemplateId, opt => opt.MapFrom(src => src.TourTemplateId))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.TourDuration.DurationName))
                .ForMember(dest => dest.TourCategory, opt => opt.MapFrom(src => src.TourCategory.Name));
            CreateMap<Province, ProvincePreview>();

        }
    }
}
