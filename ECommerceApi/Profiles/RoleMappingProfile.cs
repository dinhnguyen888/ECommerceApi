using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Profiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleGetDto>();
                    
            CreateMap<RoleUpdateDto, Role>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<RolePostDto, Role>()
              .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
