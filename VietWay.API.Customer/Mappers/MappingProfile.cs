﻿using VietWay.Repository.EntityModel;
using AutoMapper;
using VietWay.API.Customer.ResponseModel;

namespace VietWay.API.Customer.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tour, TourPreview>();
            CreateMap<Tour, TourDetail>();
            CreateMap<Repository.EntityModel.Customer, CustomerBookingInfo>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Account.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email));
        }
    }
}
