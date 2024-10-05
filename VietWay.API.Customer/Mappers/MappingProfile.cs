using AutoMapper;
using VietWay.Repository.EntityModel;
using VietWay.API.Customer.ResponseModel;

namespace VietWay.API.Customer.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tour, TourPreview>();
            CreateMap<Tour, TourDetail>();
            CreateMap<Repository.EntityModel.Customer, CustomerProfile>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.ProvinceName, opt => opt.MapFrom(src => src.Province.ProvinceName));
        }
    }
}
