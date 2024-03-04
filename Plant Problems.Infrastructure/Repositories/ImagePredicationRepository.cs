namespace Plant_Problems.Infrastructure.Repositories
{
    public class ImagePredicationRepository : GenericRepository<ImagePredication>, IImagePredicationRepositry
    {

        private readonly DbSet<ImagePredication> _image;
        private readonly ApplicationDbContext _context;
        public ImagePredicationRepository(ApplicationDbContext context) : base(context)
        {
            _image = context.Set<ImagePredication>();
            _context = context;
        }



        public async Task DeleteImagesAsync(ICollection<ImagePredication> entities)
        {
            foreach (var entity in entities)
            {
                _image.Remove(entity);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<ImagePredication>> GetImagePredicationsByUserIdAsync(string userId)
        {
            return await _image.Where(ip => ip.UserId == userId).Include(u => u.User).ToListAsync();
        }

        public async Task<List<ImagePredication>> GetImgagePredicationsListAsync()
        {
            return await _image.Include(u => u.User).ToListAsync();
        }

        public ImagePredication GetTrackedImageById(Guid imageId)
        {
            return _image
            .Include(u => u.User)
            .AsNoTracking()
            .FirstOrDefault(c => c.ID == imageId);
        }
    }
}
