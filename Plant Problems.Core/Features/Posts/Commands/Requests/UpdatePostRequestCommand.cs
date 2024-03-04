namespace Plant_Problems.Core.Features.Posts.Commands.Requests
{
    public class UpdatePostRequestCommand : AddPostRequestCommand, IRequest<Response<string>>
    {
        public Guid ID { get; set; }
    }
}
