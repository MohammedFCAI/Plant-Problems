using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plant_Problems.API.Bases;
using Plant_Problems.Core.Features.Posts.Commands.Requests;
using Plant_Problems.Core.Features.Posts.Queries.Requests;

namespace Plant_Problems.API.Controllers
{
	[Route("api/posts")]
	[ApiController]
	public class PostsController : AppControllerBase
	{
		private readonly IMediator _mediator;

		public PostsController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllPosts()
		{
			var response = await _mediator.Send(new GetPostsListRequestQuery());
			return NewResult(response);
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetPostById(Guid id)
		{
			var response = await _mediator.Send(new GetPostByIdRequestQuery(id));
			return NewResult(response);
		}

		[HttpPost]
		public async Task<IActionResult> AddPost([FromForm] AddPostRequestCommand request)
		{
			var response = await _mediator.Send(request);
			return NewResult(response);
		}

		[HttpPut]
		public async Task<IActionResult> UpdatePost([FromForm] UpdatePostRequestCommand request)
		{
			var response = await _mediator.Send(request);
			return NewResult(response);
		}

		[HttpGet("search/{content}")]
		public async Task<IActionResult> Search(string content)
		{
			var response = await _mediator.Send(new GetPostByContentRequestQuery(content));
			return NewResult(response);
		}

		[HttpDelete("{id:guid}")]
		public async Task<IActionResult> DeletePost(Guid id)
		{
			var response = await _mediator.Send(new DeletePostRequestCommand(id));
			return NewResult(response);
		}

		[HttpGet("users/{userId}")]
		public async Task<IActionResult> GetPostsByUserId(string userId)
		{
			var response = await _mediator.Send(new GetPostsByUserIdRequestQuery(userId));
			return NewResult(response);
		}

		[HttpPost("save-post")]
		public async Task<IActionResult> SavePost([FromForm] SavePostRequestCommand request)
		{
			var response = await _mediator.Send(request);
			return NewResult(response);
		}


		[HttpPost("unsave-post")]
		public async Task<IActionResult> UnSavePost([FromForm] UnSavePostRequestCommand request)
		{
			var response = await _mediator.Send(request);
			return NewResult(response);
		}


		[HttpGet("saved-posts/{userId}")]
		public async Task<IActionResult> SavePost(string userId)
		{
			var response = await _mediator.Send(new GetSavedPostsRequestQuery(userId));
			return NewResult(response);
		}
	}
}
