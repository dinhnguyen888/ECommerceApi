using AutoMapper;
using backend.Dtos;
using backend.Models;

namespace backend.Profiles
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
