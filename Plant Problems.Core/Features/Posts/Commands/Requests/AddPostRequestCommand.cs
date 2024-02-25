using Microsoft.AspNetCore.Http;

namespace Plant_Problems.Core.Features.Posts.Commands.Requests
{
	public class AddPostRequestCommand : IRequest<Response<Post>>
	{
		public string Content { get; set; }
		public IFormFile Image { get; set; }
		public string UserId { get; set; }
	}
}