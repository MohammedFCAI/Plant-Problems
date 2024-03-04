namespace Plant_Problems.Core.Features.Comments.Queries.Requests
{
    public class GetCommentsListForPost : IRequest<Response<List<GetCommentsListForPostResponseQuery>>>
    {
        public Guid PostId { get; set; }

        public GetCommentsListForPost(Guid postId)
        {
            PostId = postId;
        }
    }
}
