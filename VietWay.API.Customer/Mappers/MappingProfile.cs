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
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Pending))
                .ForMember(dest => dest.BookingTourists, opt => opt.MapFrom(src => src.TourParticipants
                    .Select(x => new BookingTourist()
                    {
                        DateOfBirth = x.DateOfBirth,
                        FullName = x.FullName,
                        Gender = x.Gender,
                        PhoneNumber = x.PhoneNumber,
                    })));
            CreateMap<CreateAccountRequest, Account>();
            CreateMap<CreateAccountRequest, Repository.EntityModel.Customer>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => new Account()
                {
                    Email = src.Email,
                    Password = src.Password,
                    PhoneNumber = src.PhoneNumber,
                    Role = UserRole.Customer,
                    IsDeleted = false,
                }));
            CreateMap<ReviewTourRequest, TourReview>();
        }
    }
}
