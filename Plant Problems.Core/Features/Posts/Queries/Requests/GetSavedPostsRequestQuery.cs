namespace Plant_Problems.Core.Features.Posts.Queries.Requests
{
    public class GetSavedPostsRequestQuery : IRequest<Response<List<GetSavedPostsResponseQuery>>>
    {
        public string UserId { get; set; }

        public GetSavedPostsRequestQuery(string userId)
        {
            UserId = userId;
        }
    }
}
