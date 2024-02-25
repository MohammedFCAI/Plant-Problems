namespace Plant_Problems.Core.Features.Users.Commands.Requests
{
	public class DeleteUserRequestCommand : IRequest<Response<string>>
	{
		public string UserId { get; set; }

		public DeleteUserRequestCommand(string userId)
		{
			UserId = userId;
		}
	}
}
