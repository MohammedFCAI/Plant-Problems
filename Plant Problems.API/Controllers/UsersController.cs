namespace Plant_Problems.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : AppControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _mediator.Send(new GetUsersListRequestQuery());
            return NewResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllUsers(string id)
        {
            var response = await _mediator.Send(new GetUserByIdRequestQuery(id));
            return NewResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromForm] DeleteUserRequestCommand user)
        {
            var response = await _mediator.Send(user);
            return NewResult(response);
        }
    }
}
