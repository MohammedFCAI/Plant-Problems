namespace Plant_Problems.Service.Interfaces
{
    public interface ICommentService
    {
        Task<ServiceResponse<List<Comment>>> GetCommentsList();

        Task<ServiceResponse<Comment>> GetCommentById(Guid commentId);

        Task<ServiceResponse<List<Comment>>> GetCommentsListForPost(Guid postId);

        Task<ServiceResponse<Comment>> AddComment(Comment comment);

        Task<ServiceResponse<Comment>> UpdateComment(Comment entity);

        Task<ServiceResponse<Comment>> DeleteComment(Comment comment);

        Task<int> GetNumberOfCommentsForPost(Guid postId);
    }
}
