using MediatR;
using Plant_Problems.Core.Bases;
using Plant_Problems.Data.Models;

namespace Plant_Problems.Core.Features.Posts.Commands.Requests
{
	public class DeletePostRequestCommand : IRequest<Response<Post>>
	{
		public Guid Id { get; set; }

		public DeletePostRequestCommand(Guid id)
		{
			Id = id;
		}
	}
}
