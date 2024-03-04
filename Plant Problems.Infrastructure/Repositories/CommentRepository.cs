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
            return await _comments.Include(u => u.User).ToListAsync();
        }

        public async Task<List<Comment>> GetCommentsListForPostAsync(Guid postId)
        {
            return await _comments.Where(i => i.PostId == postId).Include(u => u.User).ToListAsync();
        }


        public Comment GetTrackedCommentById(Guid commentId)
        {
            return _comments.Include(u => u.User)
                .AsNoTracking()
                .FirstOrDefault(c => c.ID == commentId);
        }

        public override async Task DeleteRangeAsync(ICollection<Comment> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }
            await _context.SaveChangesAsync();
        }

        public void DetachComment(Comment comment)
        {
            _context.Entry(comment).State = EntityState.Detached;
        }

    }
}
