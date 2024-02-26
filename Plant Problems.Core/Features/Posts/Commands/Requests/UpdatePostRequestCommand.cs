namespace Plant_Problems.Core.Features.Posts.Commands.Requests
{
	public class UpdatePostRequestCommand : AddPostRequestCommand, IRequest<Response<Post>>
	{
		public Guid ID { get; set; }
	}
}
