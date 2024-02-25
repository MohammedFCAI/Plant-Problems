using Plant_Problems.Core.Features.Posts.Queries.Requests;

namespace Plant_Problems.Core.Mappings.Posts
{
	public class PostProfile : Profile
	{
		public PostProfile()
		{
			AddPostMapping();
			GetPostByContent();
		}

		private void AddPostMapping()
		{

			CreateMap<AddPostRequestCommand, Post>()
				.ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
				.ForPath(dest => dest.User.Id, opt => opt.MapFrom(src => src.UserId))
				.ForMember(dest => dest.Image, opt => opt.Ignore())
				.ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
		}



		private void GetPostByContent()
		{
			CreateMap<GetPostByContentRequestQuery, Post>().ReverseMap();
		}
	}
}
