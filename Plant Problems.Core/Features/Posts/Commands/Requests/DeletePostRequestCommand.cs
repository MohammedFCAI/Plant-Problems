namespace Plant_Problems.Core.Features.Posts.Commands.Requests
{
    public class DeletePostRequestCommand : IRequest<Response<string>>
    {
        public Guid Id { get; set; }

        public DeletePostRequestCommand(Guid id)
        {
            Id = id;
        }
    }
}
