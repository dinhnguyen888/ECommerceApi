using AutoMapper;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Application.Mappings
{
    // AutoMapper profile cho Transaction Service
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            // Order mappings
            CreateMap<Order, OrderGetDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));

            // OrderItem mappings
            CreateMap<OrderItem, OrderItemGetDto>();

            // Payment mappings
            CreateMap<Payment, PaymentGetDto>();
        }
    }
}
