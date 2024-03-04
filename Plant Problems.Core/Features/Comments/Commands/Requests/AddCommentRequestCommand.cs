namespace Plant_Problems.Core.Features.Comments.Commands.Requests
{
    public class AddCommentRequestCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
    }
}
