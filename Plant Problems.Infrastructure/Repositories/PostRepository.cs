namespace Plant_Problems.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly DbSet<Post> _posts;
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _posts = context.Set<Post>();
            _context = context;
        }

        // Old Version.
        public async Task<Post> GetPostsByContent(string content)
        {

            return await _posts.FirstOrDefaultAsync(c => c.Content.Contains(content));
        }

        public IEnumerable<Post> Search(string content)
        {
            return _posts
            .Include(c => c.Comments)
            .Include(u => u.User)
            .AsEnumerable()
            .Where(p => p.Content.Contains(content, StringComparison.OrdinalIgnoreCase))
            .ToList();
        }

        public async Task<List<Post>> GetPostsListAsync()
        {
            return await _posts.Include(x => x.Comments).Include(u => u.User).ToListAsync();
        }

        public void UpdatAsync(Post post)
        {
            _posts.Update(post);
            _context.SaveChanges();
        }

        public async Task<Post> GetByIdAsync(Guid id, Func<IQueryable<Post>, IQueryable<Post>> include = null)
        {
            IQueryable<Post> query = _context.Posts;

            if (include != null)
            {
                query = include(query);
            }

            return await query.Include(u => u.User).AsNoTracking().FirstOrDefaultAsync(post => post.ID == id);
        }

        public void Detach(Post post)
        {
            if (_context.Entry(post.User).State == EntityState.Detached)
                _context.Entry(post.User).State = EntityState.Detached;
        }

        public async Task<List<Post>> GetPostsListByUserId(string userId)
        {
            var posts = await _context.Posts
                .Include(c => c.Comments)
                .Include(u => u.User)
                .Where(sp => sp.UserId == userId)
                .ToListAsync();

            return posts;
        }
    }
}
