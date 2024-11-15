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
            CreateMap<BookTourRequest, Booking>()
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.TourId, opt => opt.MapFrom(src => src.TourId))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.NumberOfParticipants, opt => opt.MapFrom(src => src.NumberOfParticipants))
                .ForMember(dest => dest.ContactFullName, opt => opt.MapFrom(src => src.ContactFullName))
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.ContactPhoneNumber, opt => opt.MapFrom(src => src.ContactPhoneNumber))
                .ForMember(dest => dest.ContactAddress, opt => opt.MapFrom(src => src.ContactAddress))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.MinValue))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.BookingTourists, opt => opt.MapFrom(src => src.TourParticipants
                    .Select(x => new BookingTourist()
                    {
                        DateOfBirth = x.DateOfBirth,
                        FullName = x.FullName,
                        Gender = x.Gender,
                        TouristId = "",
                        PhoneNumber = x.PhoneNumber,
                        BookingId = "",
                        Price = 0,
                    })));
            CreateMap<CreateAccountRequest, Account>();
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

            CreateMap<CreateAccountWithGoogleRequest, Account>();
            CreateMap<CreateAccountWithGoogleRequest, Repository.EntityModel.Customer>()
                .ForMember(dest => dest.ProvinceId, opt => opt.MapFrom(src => src.ProvinceId))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => new Account()
                {
                    AccountId = "",
                    PhoneNumber = src.PhoneNumber,
                    Role = UserRole.Customer,
                    CreatedAt = DateTime.MinValue,
                    IsDeleted = false,
                }));
        }
    }
}
