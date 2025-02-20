using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
     
        CreateMap<Comment, CommentGetDto>()
            .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies ?? new List<Comment>()));

        CreateMap<CommentPostDto, Comment>();

        CreateMap<CommentReplyDto, Comment>();
    }
}
