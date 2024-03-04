namespace Plant_Problems.Core.Features.Comments.Commands.Handlers
{
    public class CommentHandlerCommand : ResponseHandler, IRequestHandler<AddCommentRequestCommand, Response<string>>
        , IRequestHandler<UpdateCommentRequestCommand, Response<string>>, IRequestHandler<DeleteCommentRequestCommand, Response<string>>
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public CommentHandlerCommand(ICommentService commentService, IMapper mapper, IUserService userService)
        {
            _commentService = commentService;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Response<string>> Handle(AddCommentRequestCommand request, CancellationToken cancellationToken)
        {

            var userResponse = await _userService.GetUserById(request.UserId);
            if (!userResponse.Success)
                return BadRequest<string>(userResponse.Message);
            var user = userResponse.Entities;

            var commentMapping = _mapper.Map<Comment>(request);

            commentMapping.User = user;

            var commentResponse = await _commentService.AddComment(commentMapping);

            if (!commentResponse.Success)
                return NotFound<string>(commentResponse.Message);

            var comment = commentResponse.Entities;
            return Success("", commentResponse.Message, 1);
        }

        public async Task<Response<string>> Handle(UpdateCommentRequestCommand request, CancellationToken cancellationToken)
        {
            var commentMapping = _mapper.Map<Comment>(request);
            var commentResponse = await _commentService.UpdateComment(commentMapping);
            if (!commentResponse.Success)
                return NotFound<string>(commentResponse.Message);

            return Success(commentResponse.Message);
        }

        public async Task<Response<string>> Handle(DeleteCommentRequestCommand request, CancellationToken cancellationToken)
        {
            var userResponse = await _userService.GetUserById(request.UserId);
            if (!userResponse.Success)
                return BadRequest<string>(userResponse.Message);

            _userService.Detach(userResponse.Entities);
            var user = userResponse.Entities;
            var commentMapping = _mapper.Map<Comment>(request);
            commentMapping.User = user;
            var commentResponse = await _commentService.DeleteComment(commentMapping);
            if (!commentResponse.Success)
                return NotFound<string>(commentResponse.Message);

            return Success(commentResponse.Message);
        }
    }
}
