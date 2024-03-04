namespace Plant_Problems.Data.Models
{
    public class SavedPost
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
