namespace Plant_Problems.Infrastructure.Interfaces
{
    public interface IImagePredicationRepositry : IGenericRepository<ImagePredication>
    {
        public ImagePredication GetTrackedImageById(Guid imageId);
        Task DeleteImagesAsync(ICollection<ImagePredication> entities);
        Task<List<ImagePredication>> GetImagePredicationsByUserIdAsync(string userId);

        Task<List<ImagePredication>> GetImgagePredicationsListAsync();
    }
}
