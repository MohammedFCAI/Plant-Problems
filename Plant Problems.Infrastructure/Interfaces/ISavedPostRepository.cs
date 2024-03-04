namespace Plant_Problems.Infrastructure.Interfaces
{
    public interface ISavedPostRepository : IGenericRepository<SavedPost>
    {
        Task<SavedPost> SavePostAsync(Post post, ApplicationUser user);
        Task<List<Post>> GetSavedPostsAsync(string userId);
        Task<bool> UnsavePostAsync(Guid postId, string userId);
        Task DeleteRangeAsync(ICollection<Post> entities);
        public Task<bool> IsSaved(Post post, ApplicationUser user);
    }
}
