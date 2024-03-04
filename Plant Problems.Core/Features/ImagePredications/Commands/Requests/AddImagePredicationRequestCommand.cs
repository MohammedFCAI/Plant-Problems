namespace Plant_Problems.Core.Features.ImagePredications.Commands.Requests
{
    public class AddImagePredicationRequestCommand : IRequest<Response<string>>
    {
        public IFormFile? Image { get; set; }
        public string UserId { get; set; }
        public string Response { get; set; }
    }

    public class AddImageRequestCommand
    {
        public IFormFile? Image { get; set; }
        public string UserId { get; set; }
    }
}
