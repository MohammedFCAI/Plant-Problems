using Plant_Problems.Core.Features.Comments.Commands.Requests;

namespace Plant_Problems.Core.Mappings.Comments
{
	public class CommentProfile : Profile
	{
		public CommentProfile()
		{
			AddCommentMapping();
			UpdateCommentMapping();
			DeleteCommentMapping();
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
	}
}
