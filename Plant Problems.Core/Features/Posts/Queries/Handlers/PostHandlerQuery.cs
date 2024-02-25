using Plant_Problems.Core.Features.Posts.Queries.Requests;

namespace Plant_Problems.Core.Features.Posts.Queries.Handlers
{
	public class PostHandlerQuery : ResponseHandler, IRequestHandler<GetPostsListRequestQuery, Response<List<Post>>>
		, IRequestHandler<GetPostByIdRequestQuery, Response<Post>>, IRequestHandler<GetPostByContentRequestQuery, Response<List<Post>>>
		, IRequestHandler<GetPostsByUserIdRequestQuery, Response<List<Post>>>, IRequestHandler<GetSavedPostsRequestQuery, Response<List<Post>>>
	{
		private readonly IPostService _postService;
		private readonly IMapper _mapper;

		public PostHandlerQuery(IPostService postService, IMapper mapper)
		{
			_postService = postService;
			_mapper = mapper;
		}

		public async Task<Response<List<Post>>> Handle(GetPostsListRequestQuery request, CancellationToken cancellationToken)
		{
			var postsResponse = await _postService.GetPostsList();
			if (!postsResponse.Success)
				return BadRequest<List<Post>>(postsResponse.Message);
			var posts = postsResponse.Entities.ToList();
			var count = posts.Count;
			return Success(posts, postsResponse.Message, count);
		}

		public async Task<Response<Post>> Handle(GetPostByIdRequestQuery request, CancellationToken cancellationToken)
		{
			var postResponse = await _postService.GetPostById(request.Id);
			if (!postResponse.Success)
				return NotFound<Post>(postResponse.Message);
			var post = postResponse.Entities;
			return Success(post, postResponse.Message, 1);
		}

		public async Task<Response<List<Post>>> Handle(GetPostByContentRequestQuery request, CancellationToken cancellationToken)
		{
			var postResponse = _postService.SearchPosts(request.Content);

			if (!postResponse.Success)
				return NotFound<List<Post>>(postResponse.Message);
			var posts = postResponse.Entities;
			posts = posts.ToList();
			var count = posts.Count();
			return Success(posts.ToList(), postResponse.Message, count);
		}

		public async Task<Response<List<Post>>> Handle(GetPostsByUserIdRequestQuery request, CancellationToken cancellationToken)
		{
			var postResponse = await _postService.GetPostsByUserId(request.UserId);
			if (!postResponse.Success)
				return NotFound<List<Post>>(postResponse.Message);

			var posts = postResponse.Entities;
			return Success(posts, postResponse.Message, posts.Count());
		}

		public async Task<Response<List<Post>>> Handle(GetSavedPostsRequestQuery request, CancellationToken cancellationToken)
		{
			var postResponse = await _postService.GetSavedPostsAsync(request.UserId);
			if (!postResponse.Success)
				return NotFound<List<Post>>(postResponse.Message);


			var savedPosts = postResponse.Entities;
			if (savedPosts == null)
				savedPosts = new List<Post>();
			return Success(savedPosts, postResponse.Message, savedPosts.Count());
		}
	}
}