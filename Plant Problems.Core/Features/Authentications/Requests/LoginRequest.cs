namespace Plant_Problems.Core.Features.Authentications.Requests
{
	public class LoginRequest : IRequest<Response<string>>
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
