using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BannerPostDto, Banner>();
            CreateMap<BannerUpdateDto, Banner>();
        }
    }
}
