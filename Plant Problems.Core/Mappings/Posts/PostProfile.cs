namespace Plant_Problems.Core.Mappings.Posts
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            AddPostMapping();
            GetPostByContentMapping();
            UpdatePostMapping();
            GetPostsListMapping();
            GetPostByIdMapping();
            GetPostsByUserIdMapping();
            GetSavedPostsMapping();
        }

        private void AddPostMapping()
        {

            CreateMap<AddPostRequestCommand, Post>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForPath(dest => dest.User.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }

        private void UpdatePostMapping()
        {

            CreateMap<UpdatePostRequestCommand, Post>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForPath(dest => dest.User.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }

        private void GetPostByContentMapping()
        {
            CreateMap<Post, GetPostByContentResponseQuery>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));
        }

        private void GetPostsListMapping()
        {
            CreateMap<Post, GetPostsListResponseQuery>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));
        }

        private void GetPostByIdMapping()
        {
            CreateMap<Post, GetPostByIdResponseQuery>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));
        }

        private void GetPostsByUserIdMapping()
        {
            CreateMap<Post, GetPostsByUserIdResponseQuery>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));
        }

        private void GetSavedPostsMapping()
        {
            CreateMap<Post, GetSavedPostsResponseQuery>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.UserName));
        }



    }
}
