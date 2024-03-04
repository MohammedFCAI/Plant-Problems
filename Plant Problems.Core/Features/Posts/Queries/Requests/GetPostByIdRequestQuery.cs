namespace Plant_Problems.Core.Features.Posts.Queries.Requests
{
    public class GetPostByIdRequestQuery : IRequest<Response<GetPostByIdResponseQuery>>
    {
        public Guid Id { get; set; }

        public GetPostByIdRequestQuery(Guid id)
        {
            Id = id;
        }
    }
}
