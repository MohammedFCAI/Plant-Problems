namespace Plant_Problems.Core.Features.Authentications.Requests
{
    public class ForgetPasswordRequest : IRequest<Response<string>>
    {
        public string Email { get; set; }

        public ForgetPasswordRequest(string email)
        {
            Email = email;
        }
    }
}
