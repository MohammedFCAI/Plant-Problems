namespace Plant_Problems.Core.Features.Posts.Queries.Responses
{
    public class GetPostsListResponseQuery
    {
        public Guid ID { get; set; }

        public string Content { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public List<Comment> Comments { get; set; }

        public string Username { get; set; }
    }
}
