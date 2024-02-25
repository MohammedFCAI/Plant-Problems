namespace Plant_Problems.Core.Features.Comments.Commands.Requests
{
	public class DeleteCommentRequestCommand : IRequest<Response<string>>
	{
		public string UserId { get; set; }
		public Guid CommentId { get; set; }

	}
}
