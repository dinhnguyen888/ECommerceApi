using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Profiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile() 
        {
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<ProductPostDto, Product>();
            CreateMap<Product, ProductGetDto>();
        }
    }
}
