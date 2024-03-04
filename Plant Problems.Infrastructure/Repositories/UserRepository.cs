namespace Plant_Problems.Infrastructure.Repositories
{
    internal class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly DbSet<ApplicationUser> _users;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _users = context.Set<ApplicationUser>();
            _context = context;
            _userManager = userManager;

        }


        public void Detach(ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Detached;
        }

        public Task<List<ApplicationUser>> GetAllAsync()
        {
            return _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
