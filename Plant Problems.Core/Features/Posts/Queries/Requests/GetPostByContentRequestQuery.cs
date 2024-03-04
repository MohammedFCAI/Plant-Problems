namespace Plant_Problems.Core.Features.Posts.Queries.Requests
{
    public class GetPostByContentRequestQuery : IRequest<Response<List<GetPostByContentResponseQuery>>>
    {
        public string Content { get; set; }

        public GetPostByContentRequestQuery(string content)
        {
            Content = content;
        }
    }
}
