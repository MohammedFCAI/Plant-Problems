using MediatR;
using Plant_Problems.Core.Bases;
using Plant_Problems.Data.Models;

namespace Plant_Problems.Core.Features.Posts.Queries.Requests
{
	public class GetPostsListRequestQuery : IRequest<Response<List<Post>>>
	{
	}
}
