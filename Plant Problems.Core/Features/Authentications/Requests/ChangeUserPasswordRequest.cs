namespace Plant_Problems.Core.Features.Authentications.Requests
{
    public class ChangeUserPasswordRequest : IRequest<Response<string>>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
