using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plant_Problems.API.Bases;
using Plant_Problems.Core.Features.ImagePredications.Commands.Requests;
using Plant_Problems.Core.Features.ImagePredications.Queries.Requests;

namespace Plant_Problems.API.Controllers
{
	[Route("api/images")]
	[ApiController]
	public class ImagesController : AppControllerBase
	{
		private readonly IMediator _mediator;
		private new List<string> _allowedExtensions = new List<string>() { ".jpg", ".png", ".jpeg" };
		private long _maxAllowedImageSize = 5000000;

		public ImagesController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost]
		public async Task<IActionResult> UploadImage([FromForm] AddImageRequestCommand request)
		{

			if (!_allowedExtensions.Contains(Path.GetExtension(request.Image.FileName).ToLower()))
				return BadRequest("Only .png, .jpg, and .jpeg are allowed!");

			else if (request.Image.Length > _maxAllowedImageSize)
				return BadRequest("Max allowed length for the image is 5MB!");

			// Send iamge as array of bytes to the model.
			// var imageArrayOfBytes = 
			var imageArrayOfBytes = await ConvertImageToBytes(request.Image);


			var data = "This will be taked from Model.";
			var imageHandler = new AddImagePredicationRequestCommand() { Image = request.Image, UserId = request.UserId, Response = data };
			var response = await _mediator.Send(imageHandler);
			return NewResult(response);
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

		// Placeholder method to send image data to the model
		// private async Task<ModelResponse> SendImageDataToModel(byte[] imageData)
		// {
		//     // Implement your logic to send data to the model and get predictions
		//     // Example: Call a service or API that interacts with the model
		//     // Return the model response
		// }
	}
}
