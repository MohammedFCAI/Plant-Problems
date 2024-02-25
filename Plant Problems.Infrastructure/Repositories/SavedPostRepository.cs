using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Plant_Problems.Data.Models;
using Plant_Problems.Infrastructure.Interfaces;

namespace Plant_Problems.Infrastructure.Repositories
{
	public class SavedPostRepository : ISavedPostRepository
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		public SavedPostRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public async Task<List<Post>> GetSavedPostsAsync(string userId)
		{
			List<Guid> savedPostIds = await _context.SavedPosts
				.Where(sp => sp.UserId == userId)
				.Select(sp => sp.PostId)
				.ToListAsync();

			List<Post> savedPosts = await _context.Posts
				.Where(p => savedPostIds.Contains(p.ID))
				.ToListAsync();

			return savedPosts;
		}

		public async Task<SavedPost> SavePostAsync(Post post, ApplicationUser user)
		{
			var savedPost = new SavedPost() { Post = post, User = user, PostId = post.ID, UserId = user.Id };
			_context.Entry(post).State = EntityState.Unchanged;
			await _context.SavedPosts.AddAsync(savedPost);
			await _context.SaveChangesAsync();
			return savedPost;
		}

		public async Task<bool> UnsavePostAsync(Guid postId, string userId)
		{
			var savedPost = await _context.SavedPosts
				.FirstOrDefaultAsync(sp => sp.PostId == postId && sp.UserId == userId);

			if (savedPost != null)
			{
				_context.SavedPosts.Remove(savedPost);
				await _context.SaveChangesAsync();
				return true; // Successfully unsaved
			}

			return false; // Post was not saved by the user
		}
	}
}
