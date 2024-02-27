using Plant_Problems.Data.Models;
using Plant_Problems.Infrastructure.Bases;

namespace Plant_Problems.Infrastructure.Interfaces
{
	public interface IPostRepository : IGenericRepository<Post>
	{
		public Task<Post> GetByIdAsync(Guid id, Func<IQueryable<Post>, IQueryable<Post>> include = null);
		Task<List<Post>> GetPostsListAsync();
		Task<Post> GetPostsByContent(string content);
		Task<List<Post>> GetPostsListByUserId(string userId);
		//Task<List<Post>> GetSavedPostsAsync(string userId);
		IEnumerable<Post> Search(string content);
		void UpdatAsync(Post entity);
		public void Detach(Post post);

	}
}
