namespace Plant_Problems.Core.Features.Posts.Commands.Requests
{
	public class UnSavePostRequestCommand : IRequest<Response<string>>
	{
		public Guid PostId { get; set; }
		public string UserId { get; set; }
	}
}
