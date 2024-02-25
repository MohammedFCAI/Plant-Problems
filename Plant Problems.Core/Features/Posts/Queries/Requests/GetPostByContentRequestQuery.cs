using MediatR;
using Plant_Problems.Core.Bases;
using Plant_Problems.Data.Models;

namespace Plant_Problems.Core.Features.Posts.Queries.Requests
{
	public class GetPostByContentRequestQuery : IRequest<Response<List<Post>>>
	{
		public string Content { get; set; }

		public GetPostByContentRequestQuery(string content)
		{
			Content = content;
		}
	}
}
