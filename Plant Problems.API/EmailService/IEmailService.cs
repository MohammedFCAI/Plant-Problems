namespace Plant_Problems.API.EmailService
{

    public interface IEmailService
    {
        Task<ServiceResponse<string>> SendEmailAsync(Message message);
        Task<ServiceResponse<string>> ConfirmEmail(string token, string email);
        Task<string> ConfirmEmailAsync(string token, string email);

    }
}
