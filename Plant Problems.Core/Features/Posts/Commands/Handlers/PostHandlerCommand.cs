namespace Plant_Problems.Core.Features.Posts.Commands.Handlers
{
    public class PostHandlerCommand : ResponseHandler, IRequestHandler<AddPostRequestCommand, Response<string>>
        , IRequestHandler<DeletePostRequestCommand, Response<string>>
        , IRequestHandler<UpdatePostRequestCommand, Response<string>>
        , IRequestHandler<SavePostRequestCommand, Response<string>>
        , IRequestHandler<UnSavePostRequestCommand, Response<string>>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private new List<string> _allowedExtensions = new List<string>() { ".jpg", ".png", ".jpeg" };
        private long _maxAllowedImageSize = 5000000;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostHandlerCommand(IPostService postService, IMapper mapper, UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _postService = postService;
            _mapper = mapper;
            _userManager = userManager;
            _userService = userService;
        }

        public async Task<Response<string>> Handle(AddPostRequestCommand request, CancellationToken cancellationToken)
        {

            var userResponse = await _userService.GetUserById(request.UserId);
            if (!userResponse.Success)
                return BadRequest<string>(userResponse.Message);
            var user = userResponse.Entities;
            var postMapping = _mapper.Map<Post>(request);

            if (request.Image == null)
            {
                _userService.Detach(postMapping.User);

                postMapping.User = user;
                postMapping.UserId = request.UserId;

                var _postResponse = await _postService.AddPost(postMapping);

                if (!_postResponse.Success)
                    return BadRequest<string>(_postResponse.Message);

                var _post = _postResponse.Entities;

                return Success("", _postResponse.Message, 1);
            }

            if (!_allowedExtensions.Contains(Path.GetExtension(request.Image.FileName).ToLower()))
                return BadRequest<string>("Only .png, .jpg, and .jpeg are allowed!");

            else if (request.Image.Length > _maxAllowedImageSize)
                return BadRequest<string>("Max allowed length for the image is 5MB!");

            // Map request with post.
            using var dataStream = new MemoryStream();
            await request.Image.CopyToAsync(dataStream);

            postMapping = _mapper.Map<Post>(request);

            postMapping.Image = dataStream.ToArray();

            _userService.Detach(postMapping.User);

            postMapping.User = user;
            postMapping.UserId = request.UserId;

            var postResponse = await _postService.AddPost(postMapping);

            if (!postResponse.Success)
                return BadRequest<string>(postResponse.Message);

            var post = postResponse.Entities;

            return Success("", postResponse.Message, 1);
        }

        public async Task<Response<string>> Handle(DeletePostRequestCommand request, CancellationToken cancellationToken)
        {
            var postResponse = await _postService.DeletePost(request.Id);
            if (!postResponse.Success)
                return NotFound<string>(postResponse.Message);

            var post = postResponse.Entities;

            return Success("", postResponse.Message);
        }


        public async Task<Response<string>> Handle(SavePostRequestCommand request, CancellationToken cancellationToken)
        {
            var postResponse = await _postService.SavePostAsync(request.PostId, request.UserId);

            if (!postResponse.Success)
                return NotFound<string>(postResponse.Message);

            return Success(postResponse.Message);
        }

        public async Task<Response<string>> Handle(UnSavePostRequestCommand request, CancellationToken cancellationToken)
        {
            var postResponse = await _postService.UnsavePostAsync(request.PostId, request.UserId);

            if (!postResponse.Success)
                return NotFound<string>(postResponse.Message);

            return Success(postResponse.Message);
        }


        public async Task<Response<string>> Handle(UpdatePostRequestCommand request, CancellationToken cancellationToken)
        {
            var userResponse = await _userService.GetUserById(request.UserId);
            if (!userResponse.Success)
                return BadRequest<string>(userResponse.Message);

            var user = userResponse.Entities;

            // Map request with post.
            var postMapping = _mapper.Map<Post>(request);

            if (request.Image != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(request.Image.FileName).ToLower()))
                    return BadRequest<string>("Only .png, .jpg, and .jpeg are allowed!");

                else if (request.Image.Length > _maxAllowedImageSize)
                    return BadRequest<string>("Max allowed length for the image is 5MB!");

                using var dataStream = new MemoryStream();
                await request.Image.CopyToAsync(dataStream);
                postMapping.Image = dataStream.ToArray();
            }

            _userService.Detach(postMapping.User);

            postMapping.User = user;
            postMapping.UserId = request.UserId;

            var postResponse = await _postService.UpdatePost(postMapping);

            if (!postResponse.Success)
                return BadRequest<string>(postResponse.Message);

            var post = postResponse.Entities;

            return Success("", postResponse.Message, 1);
        }
    }
}
