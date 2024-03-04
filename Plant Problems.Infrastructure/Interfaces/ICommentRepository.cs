namespace Plant_Problems.Infrastructure.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsListForPostAsync(Guid postId);
        Task<List<Comment>> GetCommentsListAsync();
        Comment GetTrackedCommentById(Guid commentId);
        new Task DeleteRangeAsync(ICollection<Comment> entities);
        void DetachComment(Comment comment);
    }
}
