namespace Plant_Problems.Core.Features.Posts.Queries.Handlers
{
    public class PostHandlerQuery : ResponseHandler, IRequestHandler<GetPostsListRequestQuery, Response<List<GetPostsListResponseQuery>>>
        , IRequestHandler<GetPostByIdRequestQuery, Response<GetPostByIdResponseQuery>>, IRequestHandler<GetPostByContentRequestQuery, Response<List<GetPostByContentResponseQuery>>>
        , IRequestHandler<GetPostsByUserIdRequestQuery, Response<List<GetPostsByUserIdResponseQuery>>>, IRequestHandler<GetSavedPostsRequestQuery, Response<List<GetSavedPostsResponseQuery>>>
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostHandlerQuery(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<Response<List<GetPostsListResponseQuery>>> Handle(GetPostsListRequestQuery request, CancellationToken cancellationToken)
        {
            var postsResponse = await _postService.GetPostsList();
            if (!postsResponse.Success)
                return BadRequest<List<GetPostsListResponseQuery>>(postsResponse.Message);

            var posts = postsResponse.Entities.ToList();

            var postsMapping = _mapper.Map<List<GetPostsListResponseQuery>>(posts);

            var count = posts.Count;

            return Success(postsMapping, postsResponse.Message, count);
        }

        public async Task<Response<GetPostByIdResponseQuery>> Handle(GetPostByIdRequestQuery request, CancellationToken cancellationToken)
        {
            var postResponse = await _postService.GetPostById(request.Id);
            if (!postResponse.Success)
                return NotFound<GetPostByIdResponseQuery>(postResponse.Message);
            var post = postResponse.Entities;

            var postMapping = _mapper.Map<GetPostByIdResponseQuery>(post);

            return Success(postMapping, postResponse.Message, 1);
        }

        public async Task<Response<List<GetPostByContentResponseQuery>>> Handle(GetPostByContentRequestQuery request, CancellationToken cancellationToken)
        {
            var postResponse = _postService.SearchPosts(request.Content);

            if (!postResponse.Success)
                return NotFound<List<GetPostByContentResponseQuery>>(postResponse.Message);
            var posts = postResponse.Entities;

            var postMapping = _mapper.Map<List<GetPostByContentResponseQuery>>(posts);
            var count = posts.Count();

            return Success(postMapping.ToList(), postResponse.Message, count);
        }

        public async Task<Response<List<GetPostsByUserIdResponseQuery>>> Handle(GetPostsByUserIdRequestQuery request, CancellationToken cancellationToken)
        {
            var postResponse = await _postService.GetPostsByUserId(request.UserId);
            if (!postResponse.Success)
                return NotFound<List<GetPostsByUserIdResponseQuery>>(postResponse.Message);

            var posts = postResponse.Entities;

            var postsMapping = _mapper.Map<List<GetPostsByUserIdResponseQuery>>(posts);

            return Success(postsMapping, postResponse.Message, posts.Count());
        }

        public async Task<Response<List<GetSavedPostsResponseQuery>>> Handle(GetSavedPostsRequestQuery request, CancellationToken cancellationToken)
        {
            var postResponse = await _postService.GetSavedPostsAsync(request.UserId);
            if (!postResponse.Success)
                return NotFound<List<GetSavedPostsResponseQuery>>(postResponse.Message);


            var savedPosts = postResponse.Entities;
            if (savedPosts == null)
                savedPosts = new List<Post>();

            var savedPostsMapping = _mapper.Map<List<GetSavedPostsResponseQuery>>(savedPosts);

            return Success(savedPostsMapping, postResponse.Message, savedPosts.Count());
        }
    }
}