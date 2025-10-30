using AutoMapper;
using ECommerce.AuthService.Application.Entities;
using ECommerce.AuthService.Application.Dtos;

namespace ECommerce.AuthService.Application.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role == null ? UserRole.Client : Enum.Parse<UserRole>(src.Role, true)))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
}
