namespace Plant_Problems.Core.Features.Authentications.Requests
{
    public class SendEmailRequest : IRequest<Response<string>>
    {
        [Required(ErrorMessage = "To is required!")]
        //public IEnumerable<string> To { get; set; }
        public string To { get; set; }

        [Required(ErrorMessage = "Subject is required!")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Content is required!")]
        public string Content { get; set; }
    }
}
