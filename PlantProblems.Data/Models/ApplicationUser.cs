using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Plant_Problems.Data.Models
{
	public class ApplicationUser : IdentityUser
	{
		//1 to M ==> Post
		[JsonIgnore]
		public List<Post> Posts { get; set; }

		// 1 to M ==> Comment
		[JsonIgnore]
		public List<Comment> Comments { get; set; }

		// 1 to M ==> ImagePredication
		[JsonIgnore]
		public List<ImagePredication> ImagePredications { get; set; }

		public string? RefreshToken { get; set; }
		public DateTime? RefreshTokenExpire { get; set; }
	}
}
