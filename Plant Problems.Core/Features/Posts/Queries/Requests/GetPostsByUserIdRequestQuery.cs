namespace Plant_Problems.Core.Features.Posts.Queries.Requests
{
    public class GetPostsByUserIdRequestQuery : IRequest<Response<List<GetPostsByUserIdResponseQuery>>>
    {
        public string UserId { get; set; }

        public GetPostsByUserIdRequestQuery(string userId)
        {
            UserId = userId;
        }
    }
}
