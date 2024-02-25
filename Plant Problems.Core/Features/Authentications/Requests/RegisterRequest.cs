using Plant_Problems.Core.Features.Authentications.Validations;

namespace Plant_Problems.Core.Features.Authentications.Requests
{
	public class RegisterRequest : IRequest<Response<string>>
	{
		public string UserName { get; set; }

		[GmailAddress]
		public string Email { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

	}

	public class NewRegisterRequest
	{
		public string UserName { get; set; }

		[GmailAddress]
		public string Email { get; set; }

		public string Password { get; set; }

		public string ConfirmPassword { get; set; }

	}

}
