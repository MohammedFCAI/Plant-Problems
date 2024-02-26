﻿using Plant_Problems.Data.Models;
using Plant_Problems.Infrastructure.Bases;

namespace Plant_Problems.Infrastructure.Interfaces
{
	public interface ISavedPostRepository : IGenericRepository<SavedPost>
	{
		Task<SavedPost> SavePostAsync(Post post, ApplicationUser user);
		Task<List<Post>> GetSavedPostsAsync(string userId);
		Task<bool> UnsavePostAsync(Guid postId, string userId);
		Task DeleteRangeAsync(ICollection<Post> entities);
	}
}
