using AutoMapper;
using backend.Dtos;
using backend.Models;

namespace backend.Profiles
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
