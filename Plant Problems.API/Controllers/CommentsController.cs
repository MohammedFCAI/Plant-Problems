using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plant_Problems.API.Bases;
using Plant_Problems.Core.Features.Comments.Commands.Requests;
using Plant_Problems.Core.Features.Comments.Queries.Requests;

namespace Plant_Problems.API.Controllers
{
	[Route("api/comments")]
	[ApiController]
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
		public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequestCommand request)
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
