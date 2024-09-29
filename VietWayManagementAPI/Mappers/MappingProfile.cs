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
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.FullName))
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.TourTemplateProvinces.Select(x => x.ProvinceId).ToList()))
                .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => src.TourTemplateSchedules.Select(x => new ScheduleDetail
                {
                    DayNumber = x.DayNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Attractions = x.AttractionSchedules.Select(y => y.AttractionId).ToList()
                }).ToList()))
                .ForMember(dest=>dest.ImageUrls, opt => opt.MapFrom(src=>src.TourTemplateImages.Select(x=>x.Image.Url).ToList()));
            CreateMap<TourTemplate, TourTemplatePreview>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.FullName))
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.TourTemplateProvinces.Select(x => x.ProvinceId).ToList()))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.TourTemplateImages.Select(x => x.Image.Url).FirstOrDefault()));
            CreateMap<Province, ProvincePreview>();

        }
    }
}
