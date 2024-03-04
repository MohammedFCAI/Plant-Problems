namespace Plant_Problems.Data.Models
{
    public class Comment
    {
        [Key]
        public Guid ID { get; set; }
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public Guid PostId { get; set; }

        [JsonIgnore]
        public Post Post { get; set; }

        // User
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
