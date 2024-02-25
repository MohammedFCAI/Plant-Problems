namespace Plant_Problems.Service
{
	public class TokensResponse
	{
		public string AccessToken { get; set; }
		public DateTime AccessTokenExpireOn { get; set; }
		public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpireOn { get; set; }
	}
}
