using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Profiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {

            CreateMap<Account, AccountGetDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

            CreateMap<Account, AccountGetForTokenGithub>();

            CreateMap<Account, TokenGenerateDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)); //Map USerId from Id


            CreateMap<AccountPostDto, Account>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()) 
                .ForMember(dest => dest.Id, opt => opt.Ignore());

           
            CreateMap<AccountUpdateDto, Account>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore()); 
        }
    }
}
