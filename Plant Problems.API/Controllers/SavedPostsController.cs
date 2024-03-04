namespace Plant_Problems.API.Controllers
{
    [Route("api/saved-posts")]
    [ApiController]
    [Authorize]
    public class SavedPostsController : AppControllerBase
    {

        private readonly IMediator _mediator;

        public SavedPostsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("save")]
        public async Task<IActionResult> SavePost([FromForm] SavePostRequestCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }


        [HttpPost("unsave")]
        public async Task<IActionResult> UnSavePost([FromForm] UnSavePostRequestCommand request)
        {
            var response = await _mediator.Send(request);
            return NewResult(response);
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> SavePost(string userId)
        {
            var response = await _mediator.Send(new GetSavedPostsRequestQuery(userId));
            return NewResult(response);
        }
    }
}
