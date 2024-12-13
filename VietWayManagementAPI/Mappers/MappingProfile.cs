using AutoMapper;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.ThirdParty.VnPay;
using VietWay.Service.ThirdParty.ZaloPay;

namespace VietWay.API.Management.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAttractionRequest, Attraction>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.ContactInfo))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsDraft ? AttractionStatus.Draft : AttractionStatus.Pending));
            CreateMap<AttractionCategory, AttractionCategoryDTO>();
            CreateMap<CreateTourTemplateRequest, TourTemplate>()
                .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.TourName))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.MinPrice))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.MaxPrice))
                .ForMember(dest => dest.StartingProvince, opt => opt.MapFrom(src => src.StartingProvinceId))
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
                        Title = x.Title,
                        Description = x.Description,
                        AttractionSchedules = (x.AttractionIds ?? new()).Select(y => new AttractionSchedule()
                        {
                            AttractionId = y,
                            TourTemplateId = "",
                            DayNumber = x.DayNumber
                        }).ToList()
                    }).ToList()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsDraft ? TourTemplateStatus.Draft : TourTemplateStatus.Pending));
            CreateMap<VnPayIPNRequest, VnPayIPN>();
            CreateMap<CreateAccountRequest, Staff>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => new Account()
                {
                    AccountId = "",
                    Email = src.Email,
                    Password = src.Password,
                    PhoneNumber = src.PhoneNumber,
                    Role = UserRole.Staff,
                    CreatedAt = DateTime.MinValue,
                    IsDeleted = false,
                }));
            CreateMap<CreatePostRequest, Post>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsDraft ? PostStatus.Draft : PostStatus.Pending));
            CreateMap<CreateManagerAccountRequest, Manager>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.ManagerId, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => new Account()
                {
                    AccountId = "",
                    Email = src.Email,
                    PhoneNumber = src.PhoneNumber,
                    Role = UserRole.Manager,
                    CreatedAt = DateTime.MinValue,
                    IsDeleted = false,
                }));
            CreateMap<ActivateStaffAccountRequest, Staff>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => ""));

            CreateMap<DeactivateStaffAccountRequest, Staff>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => ""));
            CreateMap<ActivateCustomerAccountRequest, Customer>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => ""));

            CreateMap<DeactivateCustomerAccountRequest, Customer>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => ""));

            CreateMap<CreateProvinceRequest, Province>()
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateTourCategoryRequest, TourCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<RefundRequest, BookingRefund>()
                .ForMember(dest => dest.RefundNote, opt => opt.MapFrom(src=>src.Note))
                .ForMember(dest => dest.BankCode, opt => opt.MapFrom(src => src.BankCode))
                .ForMember(dest => dest.BankTransactionNumber, opt => opt.MapFrom(src => src.BankTransactionNumber))
                .ForMember(dest => dest.RefundDate, opt => opt.MapFrom(src => src.PayTime));
            
            CreateMap<CreateStaffAccountRequest, Staff>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.StaffId, opt => opt.MapFrom(src => ""))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => new Account()
                {
                    AccountId = "",
                    Email = src.Email,
                    PhoneNumber = src.PhoneNumber,
                    Role = UserRole.Staff,
                    CreatedAt = DateTime.MinValue,
                    IsDeleted = false,
                }));

            CreateMap<CreateTourRequest, Tour>()
                .ForMember(dest => dest.StartLocation, opt => opt.MapFrom(src => src.StartLocation))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.DefaultTouristPrice, opt => opt.MapFrom(src => src.DefaultTouristPrice))
                .ForMember(dest => dest.RegisterOpenDate, opt => opt.MapFrom(src => src.RegisterOpenDate))
                .ForMember(dest => dest.RegisterCloseDate, opt => opt.MapFrom(src => src.RegisterCloseDate))
                .ForMember(dest => dest.MinParticipant, opt => opt.MapFrom(src => src.MinParticipant))
                .ForMember(dest => dest.MaxParticipant, opt => opt.MapFrom(src => src.MaxParticipant))
                .ForMember(dest => dest.PaymentDeadline, opt => opt.MapFrom(src => src.PaymentDeadline))
                .ForMember(dest => dest.TourPrices, opt => opt.MapFrom(src => (src.TourPrice ?? new())
                    .Select(x => new Repository.EntityModel.TourPrice()
                    {
                        Name = x.Name,
                        Price = x.Price,
                        AgeFrom = x.AgeFrom,
                        AgeTo = x.AgeTo
                    })
                    .ToList()))
                .ForMember(dest => dest.TourRefundPolicies, opt => opt.MapFrom(src => (src.RefundPolicies ?? new())
                    .Select(x => new TourRefundPolicy()
                    {
                        RefundPercent = x.RefundPercent,
                        CancelBefore = x.CancelBefore
                    })
                    .ToList()));

            CreateMap<EditTourRequest, Tour>()
                .ForMember(dest => dest.StartLocation, opt => opt.MapFrom(src => src.StartLocation))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.DefaultTouristPrice, opt => opt.MapFrom(src => src.DefaultTouristPrice))
                .ForMember(dest => dest.RegisterOpenDate, opt => opt.MapFrom(src => src.RegisterOpenDate))
                .ForMember(dest => dest.RegisterCloseDate, opt => opt.MapFrom(src => src.RegisterCloseDate))
                .ForMember(dest => dest.MinParticipant, opt => opt.MapFrom(src => src.MinParticipant))
                .ForMember(dest => dest.MaxParticipant, opt => opt.MapFrom(src => src.MaxParticipant))
                .ForMember(dest => dest.TourPrices, opt => opt.MapFrom(src => (src.TourPrice ?? new())
                    .Select(x => new Repository.EntityModel.TourPrice()
                    {
                        Name = x.Name,
                        Price = x.Price,
                        AgeFrom = x.AgeFrom,
                        AgeTo = x.AgeTo
                    })
                    .ToList()))
                .ForMember(dest => dest.TourRefundPolicies, opt => opt.MapFrom(src => (src.RefundPolicies ?? new())
                    .Select(x => new TourRefundPolicy()
                    {
                        RefundPercent = x.RefundPercent,
                        CancelBefore = x.CancelBefore
                    })
                    .ToList()));

            CreateMap<CreateTourDurationRequest, TourDuration>()
                .ForMember(dest => dest.DurationName, opt => opt.MapFrom(src => src.DurationName))
                .ForMember(dest => dest.NumberOfDay, opt => opt.MapFrom(src => src.NumberOfDay));

            CreateMap<CreatePostCategoryRequest, PostCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            CreateMap<CreateAttractionTypeRequest, AttractionCategory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AttractionTypeName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<ZaloPayCallbackRequest, ZaloPayCallback>();
            CreateMap<ZaloPayCallbackData, CallbackData>();
        }
    }
}
