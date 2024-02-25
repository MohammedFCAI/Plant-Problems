namespace Plant_Problems.Data.Models.Authentications
{
	public class LoginResponse
	{
		public JwtToken AccessToken { get; set; }
		public JwtToken RefreshToken { get; set; }
	}
}
