namespace Plant_Problems.Core.Features.Authentications.Requests
{
    public class LogintWithJwtRequest
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required..!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required..!")]
        public string Password { get; set; }
    }
}
