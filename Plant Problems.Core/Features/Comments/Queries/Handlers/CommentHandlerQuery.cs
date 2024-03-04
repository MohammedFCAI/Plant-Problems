namespace Plant_Problems.Core.Features.Comments.Queries.Handlers
{
    public class CommentHandlerQuery : ResponseHandler, IRequestHandler<GetCommentsListForPost, Response<List<GetCommentsListForPostResponseQuery>>>
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;

        public CommentHandlerQuery(IMapper mapper, ICommentService commentService)
        {
            _mapper = mapper;
            _commentService = commentService;
        }

        public async Task<Response<List<GetCommentsListForPostResponseQuery>>> Handle(GetCommentsListForPost request, CancellationToken cancellationToken)
        {
            var commentResponse = await _commentService.GetCommentsListForPost(request.PostId);

            if (!commentResponse.Success)
                return NotFound<List<GetCommentsListForPostResponseQuery>>(commentResponse.Message);

            var comments = commentResponse.Entities;

            var commentsMapping = _mapper.Map<List<GetCommentsListForPostResponseQuery>>(comments);

            return Success(commentsMapping, commentResponse.Message, comments.Count());
        }
    }
}
