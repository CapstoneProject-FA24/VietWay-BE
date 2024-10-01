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
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.TourTemplateSchedules.Select(x => new ScheduleDetail
                {
                    DayNumber = x.DayNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Attractions = x.AttractionSchedules.Select(y => y.AttractionId).ToList()
                }).ToList()))
                .ForMember(dest=>dest.Images, opt => opt.MapFrom(src=>src.TourTemplateImages.Select(x=> new ImageDetail() 
                { 
                    ImageId = x.ImageId, Url = x.Image.Url
                }).ToList()));
            CreateMap<TourTemplate, TourTemplatePreview>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.FullName))
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.TourTemplateProvinces.Select(x => x.Province.ProvinceName).ToList()))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.TourTemplateImages.Select(x => x.Image.Url).FirstOrDefault()))
                .ForMember(dest=>dest.TourTemplateId, opt => opt.MapFrom(src=>src.TourTemplateId));
            CreateMap<Province, ProvincePreview>();

        }
    }
}
