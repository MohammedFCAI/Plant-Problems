using System.ComponentModel.DataAnnotations;

namespace Plant_Problems.Data.Models.Authentications
{
	public class Register
	{
		[Required(ErrorMessage = "Username is required")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
		public string ConfirmPassword { get; set; }
	}
}
