namespace Plant_Problems.API.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmailService(EmailConfiguration emailConfiguration, UserManager<ApplicationUser> userManager)
        {
            _emailConfiguration = emailConfiguration;
            _userManager = userManager;
        }


        public async Task<ServiceResponse<string>> SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
            return new ServiceResponse<string>() { Entities = "", Success = true, Message = "Email Sent Successfully." };
        }

        public async Task<ServiceResponse<string>> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                    return new ServiceResponse<string> { Entities = "", Success = true, Message = "Email Verified Successfully" };
            }

            return new ServiceResponse<string>() { Entities = "", Success = false, Message = "User doesn't exist!" };
        }

        // new
        public async Task<string> ConfirmEmailAsync(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return "User not found";

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return "Email verified successfully";
            else
                return "Failed to verify email";
        }




        // Private Methods.
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart { Text = message.Content };
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);

                await client.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately, e.g., log them
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
