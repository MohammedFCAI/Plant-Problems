namespace Plant_Problems.Core.Mappings.Comments
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            AddCommentMapping();
            UpdateCommentMapping();
            DeleteCommentMapping();

            GetCommentForPostMapping();
        }

        private void AddCommentMapping()
        {
            CreateMap<AddCommentRequestCommand, Comment>().ReverseMap();
        }

        private void UpdateCommentMapping()
        {
            CreateMap<UpdateCommentRequestCommand, Comment>()
                .ForMember(des => des.ID, opt => opt.MapFrom(src => src.CommentId)).ReverseMap();
        }

        private void DeleteCommentMapping()
        {
            CreateMap<DeleteCommentRequestCommand, Comment>()
                .ForMember(des => des.ID, opt => opt.MapFrom(src => src.CommentId)).ReverseMap();
        }

        private void GetCommentForPostMapping()
        {
            CreateMap<Comment, GetCommentsListForPostResponseQuery>()
                .ForMember(des => des.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(des => des.Username, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
