using Plant_Problems.Infrastructure.Interfaces;

namespace Plant_Problems.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		public IPostRepository PostRepository { get; set; }
		public ICommentRepository CommentRepository { get; set; }
		public IUserRepository UserRepository { get; set; }
		public ISavedPostRepository SavedPostRepository { get; }
		public UnitOfWork(IPostRepository postRepository, ICommentRepository commentRepository, IUserRepository userRepository, ISavedPostRepository savedPostRepository)
		{
			PostRepository = postRepository;
			CommentRepository = commentRepository;
			UserRepository = userRepository;
			SavedPostRepository = savedPostRepository;
		}
	}
}
