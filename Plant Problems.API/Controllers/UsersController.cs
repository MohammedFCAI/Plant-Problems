using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plant_Problems.API.Bases;
using Plant_Problems.Core.Features.Users.Commands.Requests;
using Plant_Problems.Core.Features.Users.Queries.Requests;

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

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var response = await _mediator.Send(new DeleteUserRequestCommand(id));
			return NewResult(response);
		}
	}
}
