using Microsoft.AspNetCore.Identity;
using Plant_Problems.Core.Features.ImagePredications.Commands.Requests;
using Plant_Problems.Service.Authentications.Interfaces;

namespace Plant_Problems.Core.Features.ImagePredications.Commands.Handlers
{
	public class ImagePredicationsHandlerCommand : ResponseHandler, IRequestHandler<AddImagePredicationRequestCommand, Response<ImagePredication>>
		, IRequestHandler<DeleteAllImagePredicationsRequestCommand, Response<string>>
	{

		private readonly IImagePredicationService _imageService;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private new List<string> _allowedExtensions = new List<string>() { ".jpg", ".png", ".jpeg" };
		private long _maxAllowedImageSize = 5000000;
		private readonly UserManager<ApplicationUser> _userManager;

		public ImagePredicationsHandlerCommand(IImagePredicationService imageService, IMapper mapper, UserManager<ApplicationUser> userManager, IUserService userService)
		{
			_imageService = imageService;
			_mapper = mapper;
			_userManager = userManager;
			_userService = userService;
		}

		public async Task<Response<ImagePredication>> Handle(AddImagePredicationRequestCommand request, CancellationToken cancellationToken)
		{
			var userResponse = await _userService.GetUserById(request.UserId);
			if (!userResponse.Success)
				return BadRequest<ImagePredication>(userResponse.Message);
			var user = userResponse.Entities;


			var imageMapping = _mapper.Map<ImagePredication>(request);


			// Map request with Image.
			using var dataStream = new MemoryStream();
			await request.Image.CopyToAsync(dataStream);


			imageMapping.Image = dataStream.ToArray();

			_userService.Detach(imageMapping.User);

			imageMapping.User = user;
			imageMapping.UserId = request.UserId;

			var imageResponse = await _imageService.AddImage(imageMapping);

			if (!imageResponse.Success)
				return BadRequest<ImagePredication>(imageResponse.Message);

			var iamge = imageResponse.Entities;

			return Success(iamge, imageResponse.Message, 1);
		}

		public async Task<Response<string>> Handle(DeleteAllImagePredicationsRequestCommand request, CancellationToken cancellationToken)
		{
			var imageResponse = await _imageService.DeleteImagePrdications(request.UserId);

			if (!imageResponse.Success)
				return BadRequest<string>(imageResponse.Message);

			return Deleted<string>(imageResponse.Message);
		}
	}
}
