namespace Plant_Problems.Data.Models
{
    public class ImagePredication
    {
        [Key]
        public Guid ID { get; set; }

        [Required(ErrorMessage = "You must upload an image!")]
        public byte[] Image { get; set; }
        public string ImageUrl { get; set; }
        public string Prdication { get; set; }
        public DateTime CreatedOn { get; set; }


        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser User { get; set; }
    }
}
