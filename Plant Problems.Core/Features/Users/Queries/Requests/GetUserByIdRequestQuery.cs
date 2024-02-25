namespace Plant_Problems.Core.Features.Users.Queries.Requests
{
	public class GetUserByIdRequestQuery : IRequest<Response<ApplicationUser>>
	{
		public string UserId { get; set; }

		public GetUserByIdRequestQuery(string userId)
		{
			UserId = userId;
		}
	}
}
