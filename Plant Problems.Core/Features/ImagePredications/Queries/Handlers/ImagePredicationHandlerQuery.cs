using Plant_Problems.Core.Features.ImagePredications.Queries.Requests;
using Plant_Problems.Service.Authentications.Interfaces;

namespace Plant_Problems.Core.Features.ImagePredications.Queries.Handlers
{
	public class ImagePredicationHandlerQuery : ResponseHandler, IRequestHandler<GetImeagesPredicationRequestQuery, Response<List<ImagePredication>>>
	{

		private readonly IImagePredicationService _imageService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public ImagePredicationHandlerQuery(IImagePredicationService imageService, IUserService userService, IMapper mapper)
		{
			this._imageService = imageService;
			_userService = userService;
			_mapper = mapper;
		}

		public async Task<Response<List<ImagePredication>>> Handle(GetImeagesPredicationRequestQuery request, CancellationToken cancellationToken)
		{
			var iamgeResponse = await _imageService.GetImagePricationsByUserId(request.UserId);

			if (!iamgeResponse.Success)
				return BadRequest<List<ImagePredication>>(iamgeResponse.Message);
			return Success(iamgeResponse.Entities, iamgeResponse.Message, iamgeResponse.Entities.Count());
		}
	}
}
