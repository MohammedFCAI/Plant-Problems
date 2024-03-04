namespace Plant_Problems.Service.Interfaces
{
    public interface IPostService
    {
        Task<ServiceResponse<List<Post>>> GetPostsList();
        Task<ServiceResponse<Post>> GetPostById(Guid postId);
        Task<ServiceResponse<Post>> AddPost(Post post);
        Task<ServiceResponse<Post>> DeletePost(Guid postId);
        Task<ServiceResponse<Post>> UpdatePost(Post entity);
        ServiceResponse<IEnumerable<Post>> SearchPosts(string content);
        public void Dett(Post post);
        int GetNumberOfPosts();


        Task<ServiceResponse<List<Post>>> GetPostsByUserId(string userId);

        public Task<ServiceResponse<Post>> SavePostAsync(Guid postId, string userId);
        Task<ServiceResponse<List<Post>>> GetSavedPostsAsync(string userId);
        Task<ServiceResponse<Post>> UnsavePostAsync(Guid postId, string userId);
    }
}
