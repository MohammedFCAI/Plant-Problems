namespace Plant_Problems.Core.Features.ImagePredications.Queries.Handlers
{
    public class ImagePredicationHandlerQuery : ResponseHandler, IRequestHandler<GetImeagesPredicationRequestQuery, Response<List<GetImagePredictionRequestQuery>>>
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

        public async Task<Response<List<GetImagePredictionRequestQuery>>> Handle(GetImeagesPredicationRequestQuery request, CancellationToken cancellationToken)
        {
            var iamgeResponse = await _imageService.GetImagePricationsByUserId(request.UserId);

            if (!iamgeResponse.Success)
                return BadRequest<List<GetImagePredictionRequestQuery>>(iamgeResponse.Message);

            var images = iamgeResponse.Entities;

            var imageMapping = _mapper.Map<List<GetImagePredictionRequestQuery>>(images);

            return Success(imageMapping, iamgeResponse.Message, iamgeResponse.Entities.Count());
        }
    }
}
