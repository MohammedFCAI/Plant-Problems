using MediatR;
using Plant_Problems.Core.Bases;
using Plant_Problems.Data.Models;

namespace Plant_Problems.Core.Features.Posts.Queries.Requests
{
	public class GetPostByIdRequestQuery : IRequest<Response<Post>>
	{
		public Guid Id { get; set; }

		public GetPostByIdRequestQuery(Guid id)
		{
			Id = id;
		}
	}
}
