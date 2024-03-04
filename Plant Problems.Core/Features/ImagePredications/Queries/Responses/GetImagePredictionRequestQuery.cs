namespace Plant_Problems.Core.Features.ImagePredications.Queries.Responses
{
    public class GetImagePredictionRequestQuery
    {
        [Key]
        public Guid ID { get; set; }

        public string ImageUrl { get; set; }

        public string Prdication { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Username { get; set; }
    }
}
