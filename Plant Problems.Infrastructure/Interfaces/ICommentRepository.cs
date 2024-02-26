using Plant_Problems.Data.Models;
using Plant_Problems.Infrastructure.Bases;

namespace Plant_Problems.Infrastructure.Interfaces
{
	public interface ICommentRepository : IGenericRepository<Comment>
	{
		Task<List<Comment>> GetCommentsListForPostAsync(Guid postId);
		Task<List<Comment>> GetCommentsListAsync();
		public Comment GetTrackedCommentById(Guid commentId);
		new Task DeleteRangeAsync(ICollection<Comment> entities);
	}
}
