using Plant_Problems.Data.Models.Configurations;
using Plant_Problems.Service;

namespace Plant_Problems.API.EmailService
{

	public interface IEmailService
	{
		Task<ServiceResponse<string>> SendEmailAsync(Message message);
		Task<ServiceResponse<string>> ConfirmEmail(string token, string email);

	}
}
