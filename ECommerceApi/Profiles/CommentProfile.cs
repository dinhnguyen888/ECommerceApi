using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Models;

namespace ECommerceApi.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentGetDto>();
            CreateMap<CommentPostDto, Comment>();
            CreateMap<CommentUpdateDto, Comment>();
        }
    }
}
