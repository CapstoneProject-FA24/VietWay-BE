using VietWay.Repository.EntityModel;
using AutoMapper;
using VietWay.API.Customer.ResponseModel;
using VietWay.API.Customer.RequestModel;
using VietWay.Repository.EntityModel.Base;
namespace VietWay.API.Customer.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tour, TourPreview>();
            CreateMap<Tour, TourDetail>();
            CreateMap<TourTemplate, TourTemplatePreview>()
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.TourTemplateProvinces.Select(x => x.Province.ProvinceName).ToList()))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.TourTemplateImages.Select(x => x.ImageUrl).FirstOrDefault()))
                .ForMember(dest => dest.TourTemplateId, opt => opt.MapFrom(src => src.TourTemplateId))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.TourDuration.DurationName))
                .ForMember(dest => dest.TourCategory, opt => opt.MapFrom(src => src.TourCategory.Name));

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
                    Attractions = x.AttractionSchedules.Select(y => new AttractionBriefPreview()
                    {
                        AttractionId = y.AttractionId,
                        Name = y.Attraction.Name
                    }).ToList()
                }).ToList()))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.TourTemplateImages.Select(x => new ImageDetail()
                {
                    ImageId = x.ImageId,
                    Url = x.ImageUrl
                }).ToList()));
            CreateMap<BookTourRequest, Booking>();
            CreateMap<CreateAccountRequest, Account>();
            CreateMap<TourParticipant, BookingTourParticipant>();
            CreateMap<CreateAccountRequest, Repository.EntityModel.Customer>()
                .ForMember(dest => dest.ProvinceId, opt => opt.MapFrom(src => src.ProvinceId))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => new Account()
                {
                    AccountId = "",
                    Email = src.Email,
                    Password = src.Password,
                    PhoneNumber = src.PhoneNumber,
                    Role = UserRole.Customer,
                    CreatedAt = DateTime.MinValue,
                    IsDeleted = false,
                }));
        }
    }
}
