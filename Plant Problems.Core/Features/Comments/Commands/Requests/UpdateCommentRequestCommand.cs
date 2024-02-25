namespace Plant_Problems.Core.Features.Comments.Commands.Requests
{
	public class UpdateCommentRequestCommand : IRequest<Response<string>>
	{
		public Guid CommentId { get; set; }
		public Guid PostId { get; set; }
		public string Content { get; set; }
	}
}
