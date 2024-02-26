using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Plant_Problems.Data.Models
{
	public class Post
	{
		[Key]
		public Guid ID { get; set; }
		public string Content { get; set; }

		public byte[]? Image { get; set; }
		public string? ImageUrl { get; set; }

		public DateTime CreatedOn { get; set; }
		public DateTime? LastUpdatedOn { get; set; }

		public List<Comment> Comments { get; set; }

		[JsonIgnore]
		public ApplicationUser User { get; set; }
		public string UserId { get; set; }
	}
}
