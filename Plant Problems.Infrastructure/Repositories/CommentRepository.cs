using Microsoft.EntityFrameworkCore;
using Plant_Problems.Data.Models;
using Plant_Problems.Infrastructure.Bases;
using Plant_Problems.Infrastructure.Interfaces;

namespace Plant_Problems.Infrastructure.Repositories
{
	public class CommentRepository : GenericRepository<Comment>, ICommentRepository
	{
		private readonly DbSet<Comment> _comments;
		private readonly ApplicationDbContext _context;
		public CommentRepository(ApplicationDbContext context) : base(context)
		{
			_comments = context.Set<Comment>();
			_context = context;
		}

		public async Task<List<Comment>> GetCommentsListAsync()
		{
			return await _comments.ToListAsync();
		}

		public async Task<List<Comment>> GetCommentsListForPostAsync(Guid postId)
		{
			return await _comments.Where(i => i.PostId == postId).ToListAsync();
		}


		public Comment GetTrackedCommentById(Guid commentId)
		{
			return _comments
				.AsNoTracking()
				.FirstOrDefault(c => c.ID == commentId);
		}

	}
}
