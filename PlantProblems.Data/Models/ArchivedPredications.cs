using System.ComponentModel.DataAnnotations;

namespace Plant_Problems.Data.Models
{
	public class ArchivedPredications
	{
		[Key]
		public Guid ID { get; set; }
		public byte[]? Image { get; set; }
		public string ImageUrl { get; set; }
		public string? Prdication { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
