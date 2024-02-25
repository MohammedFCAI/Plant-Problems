using Microsoft.AspNetCore.Identity;
using Plant_Problems.Service.Authentications.Interfaces;

namespace Plant_Problems.Core.Features.Posts.Commands.Handlers
{
	public class PostHandlerCommand : ResponseHandler, IRequestHandler<AddPostRequestCommand, Response<Post>>
		, IRequestHandler<DeletePostRequestCommand, Response<Post>>
		, IRequestHandler<SavePostRequestCommand, Response<string>>
		, IRequestHandler<UnSavePostRequestCommand, Response<string>>
	{
		private readonly IPostService _postService;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private new List<string> _allowedExtenstions = new List<string>() { ".jpg", ".png", ".jpeg" };
		private long _maxAllowedImageSize = 5000000;
		private readonly UserManager<ApplicationUser> _userManager;
		public PostHandlerCommand(IPostService postService, IMapper mapper, UserManager<ApplicationUser> userManager, IUserService userService)
		{
			_postService = postService;
			_mapper = mapper;
			_userManager = userManager;
			_userService = userService;
		}

		public async Task<Response<Post>> Handle(AddPostRequestCommand request, CancellationToken cancellationToken)
		{

			var userResponse = await _userService.GetUserById(request.UserId);
			if (!userResponse.Success)
				return BadRequest<Post>(userResponse.Message);
			var user = userResponse.Entities;

			if (!_allowedExtenstions.Contains(Path.GetExtension(request.Image.FileName).ToLower()))
				return BadRequest<Post>("Only .png, .jpg, and .jpeg are allowed!");

			else if (request.Image.Length > _maxAllowedImageSize)
				return BadRequest<Post>("Max allowed length for the image is 5MB!");

			// Map request with post.
			using var dataStream = new MemoryStream();
			await request.Image.CopyToAsync(dataStream);

			var postMapping = _mapper.Map<Post>(request);

			postMapping.Image = dataStream.ToArray();

			_userService.Detach(postMapping.User);

			postMapping.User = user;
			postMapping.UserId = request.UserId;

			var postResponse = await _postService.AddPost(postMapping);

			if (!postResponse.Success)
				return BadRequest<Post>(postResponse.Message);

			var post = postResponse.Entities;

			return Success(post, postResponse.Message, 1);
		}

		public async Task<Response<Post>> Handle(DeletePostRequestCommand request, CancellationToken cancellationToken)
		{
			var postResponse = await _postService.DeletePost(request.Id);
			if (!postResponse.Success)
				return NotFound<Post>(postResponse.Message);

			var post = postResponse.Entities;
			return Success(post, postResponse.Message);
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
	}
}
