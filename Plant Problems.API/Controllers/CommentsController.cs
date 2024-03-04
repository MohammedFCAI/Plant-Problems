namespace Plant_Problems.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    [Authorize]
    public class CommentsController : AppControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromForm] AddCommentRequestCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpGet("{postId:guid}")]
        public async Task<IActionResult> GetCommentsListForPost(Guid postId)
        {
            var response = await _mediator.Send(new GetCommentsListForPost(postId));
            return NewResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment([FromForm] UpdateCommentRequestCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComment([FromForm] DeleteCommentRequestCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }
    }
}
