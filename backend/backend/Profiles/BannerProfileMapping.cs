using AutoMapper;
using backend.Dtos;
using backend.Models;

namespace backend.Profiles
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
