namespace Plant_Problems.Core.Features.Comments.Queries.Responses
{
    public class CommentBaseResponse
    {
        [Key]
        public Guid ID { get; set; }
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public Guid PostId { get; set; }
        public string Username { get; set; }
    }
}
