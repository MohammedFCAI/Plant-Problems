using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plant_Problems.Data.Models;
using Plant_Problems.Infrastructure.Interfaces;
using Plant_Problems.Service.Interfaces;

namespace Plant_Problems.Service.Implementations
{
	public class CommentService : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CommentService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<ServiceResponse<Comment>> AddComment(Comment comment)
		{
			var post = await _unitOfWork.PostRepository.GetByIdAsync(comment.PostId, include => include.Include(p => p.Comments));

			if (post == null)
				return new ServiceResponse<Comment>() { Entities = null, Success = false, Message = "Post id not found!" };

			if (post.Comments.IsNullOrEmpty())
				post.Comments = new List<Comment>();
			comment.CreatedOn = DateTime.Now;

			post.Comments.Add(comment);
			await _unitOfWork.CommentRepository.AddAsync(comment);
			return new ServiceResponse<Comment>() { Entities = comment, Success = true, Message = "Comment Added." };
		}

		public async Task<ServiceResponse<Comment>> DeleteComment(Guid commentId, Guid postId)
		{
			var commentResponse = await GetCommentById(commentId);
			var post = await _unitOfWork.PostRepository.GetByIdAsync(postId, include => include.Include(p => p.Comments));

			var comment = commentResponse.Entities;

			if (comment == null)
				return new ServiceResponse<Comment>() { Entities = null, Success = false, Message = "Comment id not found!" };
			if (comment.PostId != postId)
				return new ServiceResponse<Comment>() { Entities = null, Success = false, Message = "Post id not match with comment post id!" };

			post.Comments.Remove(comment);
			await _unitOfWork.CommentRepository.DeleteAsnc(comment);
			return new ServiceResponse<Comment>() { Entities = comment, Success = true, Message = "Comment deleted." };
		}

		public async Task<ServiceResponse<Comment>> DeleteComment(Comment entity)
		{

			var comment = _unitOfWork.CommentRepository.GetTrackedCommentById(entity.ID);

			if (comment == null)
				return new ServiceResponse<Comment>() { Entities = null, Success = false, Message = "Comment id not found!" };

			if (comment.UserId != entity.UserId)
				return new ServiceResponse<Comment>() { Entities = null, Success = false, Message = "User dosen't has this comment!" };

			if (comment == null)
				return new ServiceResponse<Comment>() { Entities = null, Success = false, Message = "Comment id not found!" };

			var post = await _unitOfWork.PostRepository.GetByIdAsync(comment.PostId, include => include.Include(i => i.Comments));

			post.Comments.Remove(comment);
			await _unitOfWork.CommentRepository.DeleteAsnc(comment);
			return new ServiceResponse<Comment>() { Entities = comment, Success = true, Message = "Comment deleted." };
		}

		public async Task<ServiceResponse<Comment>> GetCommentById(Guid commentId)
		{
			var comment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId);


			if (comment == null)
				return new ServiceResponse<Comment>() { Entities = null, Success = false, Message = "Comment id not found!" };
			return new ServiceResponse<Comment>() { Entities = comment, Success = true, Message = "Comment found." };
		}

		public async Task<ServiceResponse<List<Comment>>> GetCommentsList()
		{
			var comments = await _unitOfWork.CommentRepository.GetCommentsListAsync();
			if (comments == null)
				return new ServiceResponse<List<Comment>>() { Entities = null, Success = true, Message = "No comments." };

			return new ServiceResponse<List<Comment>>() { Entities = comments, Success = true, Message = "Comments found." };
		}

		public async Task<ServiceResponse<List<Comment>>> GetCommentsListForPost(Guid postId)
		{
			var post = await _unitOfWork.PostRepository.GetByIdAsync(postId, include => include.Include(p => p.Comments));
			if (post == null)
				return new ServiceResponse<List<Comment>>() { Entities = null, Success = false, Message = "Post not found!" };
			else if (post.Comments.IsNullOrEmpty())
			{
				post.Comments = new List<Comment>();
				return new ServiceResponse<List<Comment>>() { Entities = post.Comments, Success = true, Message = "No comments." };
			}

			var commentsForPost = post.Comments;
			return new ServiceResponse<List<Comment>>() { Entities = commentsForPost, Success = true, Message = "Comments found." };
		}

		public async Task<int> GetNumberOfCommentsForPost(Guid postId)
		{
			var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
			if (post == null)
				return 0;
			var comments = post.Comments;
			return comments.Count();
		}

		public async Task<ServiceResponse<Comment>> UpdateComment(Comment entity)
		{
			var comment = _unitOfWork.CommentRepository.GetTrackedCommentById(entity.ID);

			if (comment == null)
				return new ServiceResponse<Comment>() { Entities = null, Success = true, Message = "Comment is not found!" };

			else if (comment.PostId != entity.PostId)
				return new ServiceResponse<Comment>() { Entities = null, Success = false, Message = "Post is not found!" };

			await _unitOfWork.CommentRepository.UpdatAsync(entity);
			return new ServiceResponse<Comment>() { Entities = entity, Success = true, Message = "Comment updated." };
		}
	}
}
