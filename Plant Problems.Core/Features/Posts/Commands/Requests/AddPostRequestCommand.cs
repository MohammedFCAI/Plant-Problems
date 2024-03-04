namespace Plant_Problems.Core.Features.Posts.Commands.Requests
{
    public class AddPostRequestCommand : IRequest<Response<string>>
    {
        public string Content { get; set; }
        public IFormFile? Image { get; set; }
        public string UserId { get; set; }
    }
}