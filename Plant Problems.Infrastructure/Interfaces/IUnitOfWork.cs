namespace Plant_Problems.Infrastructure.Interfaces
{
	public interface IUnitOfWork
	{
		IPostRepository PostRepository { get; }
		ICommentRepository CommentRepository { get; }
		IUserRepository UserRepository { get; }
		ISavedPostRepository SavedPostRepository { get; }
		IImagePredicationRepositry ImagePredicationRepositry { get; }
	}
}
