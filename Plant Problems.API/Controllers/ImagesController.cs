using Newtonsoft.Json.Linq;

namespace Plant_Problems.API.Controllers
{
    [Route("api/images")]
    [ApiController]
    //[Authorize]
    public class ImagesController : AppControllerBase
    {
        private readonly IMediator _mediator;
        private new List<string> _allowedExtensions = new List<string>() { ".jpg", ".png", ".jpeg" };
        private long _maxAllowedImageSize = 5000000;
        private readonly HttpClient _httpClient;

        public ImagesController(IMediator mediator, HttpClient httpClient)
        {
            _mediator = mediator;
            _httpClient = httpClient;
        }


        [HttpPost("predict")]
        public async Task<IActionResult> Predict([FromForm] AddImageRequestCommand request)
        {
            if (request.Image == null || request.Image.Length == 0)
                return BadRequest("Image file is missing");

            if (!_allowedExtensions.Contains(Path.GetExtension(request.Image.FileName).ToLower()))
                return BadRequest("Only .png, .jpg, and .jpeg are allowed!");

            if (request.Image.Length > _maxAllowedImageSize)
                return BadRequest("Max allowed length for the image is 5MB!");

            var imageArrayOfBytes = await ConvertImageToBytes(request.Image);

            using (var content = new ByteArrayContent(imageArrayOfBytes))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                try
                {
                    var response = await _httpClient.PostAsync("https://web-production-94b25.up.railway.app/predict", content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseString);
                    var className = jsonResponse.GetValue("class_name")?.ToString();

                    var imageHandler = new AddImagePredicationRequestCommand() { Image = request.Image, UserId = request.UserId, Response = className };
                    var res = await _mediator.Send(imageHandler);
                    return NewResult(res);
                }
                catch (Exception ex)
                {
                    return BadRequest(new APIResponse { StatusCode = 503, Status = "Faild", ErrorMessages = "Error!! Plase try agin later!" });
                }
            }

        }





        [HttpGet("{userId}")]
        public async Task<IActionResult> GetArchivedByUseId(string userId)
        {
            var response = await _mediator.Send(new GetImeagesPredicationRequestQuery(userId));
            return NewResult(response);
        }



        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAllArchivedPredicationsByUseId(string userId)
        {
            var response = await _mediator.Send(new DeleteAllImagePredicationsRequestCommand(userId));
            return NewResult(response);
        }


        // Private Methods:
        private async Task<byte[]> ConvertImageToBytes(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

    }
}
