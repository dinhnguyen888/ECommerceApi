using AutoMapper;
using backend.Dtos;
using backend.Models;

namespace backend.Profiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile() 
        {
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<ProductPostDto, Product>();
            CreateMap<Product, ProductGetDto>();
            CreateMap<Product, ProductGetDetailDto>();
        }
    }
}
