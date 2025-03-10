﻿using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Profiles
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
         
            CreateMap<PaymentPostDto, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore()) 
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // will assign when find account
                .ForMember(dest => dest.PaymentStatus, opt => opt.Ignore());

       
            CreateMap<Payment, PaymentGetDto>();

            CreateMap<Payment, PaymentViewHistoryDto>();

            CreateMap<PaymentUpdateDto, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore()); 
        }
    }
}
