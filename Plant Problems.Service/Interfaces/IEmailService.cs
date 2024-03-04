namespace Plant_Problems.Service.Interfaces
{
    public interface IEmailService
    {
        Task<ServiceResponse<string>> SendEmailAsync(Message message);
        Task<ServiceResponse<string>> ConfirmEmail(string token, string email);
    }
}
