using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Profiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryPostDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 

            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); 
       
        }
    }
}
