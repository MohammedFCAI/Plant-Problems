using Plant_Problems.Data.Models;

namespace Plant_Problems.Infrastructure.Interfaces
{
	public interface ISavedPostRepository
	{
		Task<SavedPost> SavePostAsync(Post post, ApplicationUser user);
		Task<List<Post>> GetSavedPostsAsync(string userId);
		Task<bool> UnsavePostAsync(Guid postId, string userId);
	}
}
