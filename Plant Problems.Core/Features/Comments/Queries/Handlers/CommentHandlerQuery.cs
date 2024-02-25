using Plant_Problems.Core.Features.Comments.Queries.Requests;

namespace Plant_Problems.Core.Features.Comments.Queries.Handlers
{
	public class CommentHandlerQuery : ResponseHandler, IRequestHandler<GetCommentsListForPost, Response<List<Comment>>>
	{
		private readonly IMapper _mapper;
		private readonly ICommentService _commentService;

		public CommentHandlerQuery(IMapper mapper, ICommentService commentService)
		{
			_mapper = mapper;
			_commentService = commentService;
		}

		public async Task<Response<List<Comment>>> Handle(GetCommentsListForPost request, CancellationToken cancellationToken)
		{
			var commentResponse = await _commentService.GetCommentsListForPost(request.PostId);

			if (!commentResponse.Success)
				return NotFound<List<Comment>>(commentResponse.Message);

			var comments = commentResponse.Entities;
			return Success(comments, commentResponse.Message, comments.Count());
		}
	}
}
