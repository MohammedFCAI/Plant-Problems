using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plant_Problems.Data.Models;
using Plant_Problems.Infrastructure.Interfaces;
using Plant_Problems.Service.Interfaces;

namespace Plant_Problems.Service.Implementations
{
	public class PostService : IPostService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHostingEnvironment _hostEnvironment;

		public PostService(IUnitOfWork unitOfWork, IHostingEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
		}

		public async Task<ServiceResponse<Post>> AddPost(Post post)
		{
			if (post.Image == null)
			{

				post.ImageUrl = null;
				post.CreatedOn = DateTime.Now;
				await _unitOfWork.PostRepository.AddAsync(post);

				return new ServiceResponse<Post>() { Entities = post, Success = true, Message = "Post added successfully" };
			}
			else
				try
				{

					// Save the image to the server
					string imagePath = await SaveImage(post.Image, _hostEnvironment);

					// Update the ImageUrl property with the path where the image is stored
					post.ImageUrl = imagePath;
					post.CreatedOn = DateTime.Now;

					// Save the post to the database
					await _unitOfWork.PostRepository.AddAsync(post);

					// Return a success response with the updated post
					return new ServiceResponse<Post>() { Entities = post, Success = true, Message = "Post added successfully" };
				}
				catch (Exception ex)
				{
					// Handle any exceptions that might occur during the process
					return new ServiceResponse<Post>() { Entities = null, Success = false, Message = $"Error adding post: {ex.Message}" };
				}
		}



		public async Task<ServiceResponse<Post>> UpdatePost(Post entity)
		{
			var post = await _unitOfWork.PostRepository.GetByIdAsync(entity.ID);

			if (post == null)
				return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "Post Id not found!" };
			await DeleteImage(post.ImageUrl, _hostEnvironment);

			if (entity.Image != null)
			{
				try
				{
					// Save the image to the server
					string imagePath = await SaveImage(entity.Image, _hostEnvironment);

					// Update the ImageUrl property with the path where the image is stored
					entity.ImageUrl = imagePath;
					entity.CreatedOn = post.CreatedOn;
					entity.LastUpdatedOn = DateTime.Now;

					_unitOfWork.PostRepository.UpdatAsync(entity);

					return new ServiceResponse<Post>() { Entities = entity, Success = true, Message = "Post Updated successfully" };
				}
				catch (Exception ex)
				{
					return new ServiceResponse<Post>() { Entities = null, Success = false, Message = $"Error adding post: {ex.Message}" };
				}
			}


			//  if user doesn't want to upload imge
			entity.ImageUrl = null;
			entity.CreatedOn = post.CreatedOn;
			entity.LastUpdatedOn = DateTime.Now;

			_unitOfWork.PostRepository.UpdatAsync(entity);
			return new ServiceResponse<Post>() { Entities = entity, Success = true, Message = "Post updated." };
		}




		public async Task<ServiceResponse<Post>> DeletePost(Guid postId)
		{
			var post = await _unitOfWork.PostRepository.GetByIdAsync(postId, i => i.Include(c => c.User));

			if (post == null)
				return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "Post Id not found!" };


			await _unitOfWork.PostRepository.DeleteAsnc(post);
			await DeleteImage(post.ImageUrl, _hostEnvironment);
			return new ServiceResponse<Post>() { Entities = post, Success = true, Message = "Post Deleted Successfully" };
		}

		public int GetNumberOfPosts()
		{
			return _unitOfWork.PostRepository.GetCount();
		}

		public async Task<ServiceResponse<Post>> GetPostById(Guid postId)
		{
			var post = await _unitOfWork.PostRepository.GetByIdAsync(postId, include => include.Include(p => p.Comments));

			if (post == null)
				return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "Post not found!" };

			return new ServiceResponse<Post>() { Entities = post, Success = true, Message = "Post found" };
		}

		public async Task<ServiceResponse<List<Post>>> GetPostsList()
		{
			var posts = await _unitOfWork.PostRepository.GetPostsListAsync();
			if (posts == null || !posts.Any())
			{
				posts = new List<Post>();
				return new ServiceResponse<List<Post>>() { Entities = posts, Success = true, Message = "No posts found!" };
			}

			return new ServiceResponse<List<Post>>() { Entities = posts, Success = true, Message = "Posts found" };
		}

		public async Task<ServiceResponse<Post>> SearchByContent(string content)
		{

			var posts = await _unitOfWork.PostRepository.GetPostsByContent(content);

			if (posts == null)
				return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "Posts not found!" };

			return new ServiceResponse<Post>() { Entities = posts, Success = true, Message = "Posts found" };
		}

		public ServiceResponse<IEnumerable<Post>> SearchPosts(string content)
		{
			var posts = _unitOfWork.PostRepository.Search(content);
			if (posts.IsNullOrEmpty())
				return new ServiceResponse<IEnumerable<Post>>
				{
					Entities = null,
					Success = false,
					Message = "Posts not found!"
				};

			return new ServiceResponse<IEnumerable<Post>>
			{
				Entities = posts,
				Success = true,
				Message = "Posts found"
			};
		}


		public async Task<ServiceResponse<List<Post>>> GetPostsByUserId(string userId)
		{
			var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
			if (user == null)
				return new ServiceResponse<List<Post>>() { Entities = null, Success = false, Message = "User is not found!" };

			var posts = await _unitOfWork.PostRepository.GetPostsListByUserId(userId);

			if (!posts.Any())
			{
				posts = new List<Post>();
				return new ServiceResponse<List<Post>>() { Entities = posts, Success = true, Message = "No posts." };
			}

			return new ServiceResponse<List<Post>>() { Entities = posts, Success = true, Message = "User posts found." };
		}

		public async Task<ServiceResponse<Post>> SavePostAsync(Guid postId, string userId)
		{
			var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
			if (post == null)
				return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "Post not found!" };

			var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
			if (user == null)
				return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "User not found!" };

			await _unitOfWork.SavedPostRepository.SavePostAsync(post, user);
			return new ServiceResponse<Post>() { Entities = null, Success = true, Message = "Post saved." };
		}

		public async Task<ServiceResponse<List<Post>>> GetSavedPostsAsync(string userId)
		{
			var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
			if (user == null)
				return new ServiceResponse<List<Post>>() { Entities = null, Success = false, Message = "User is not found" };

			var savedPosts = await _unitOfWork.SavedPostRepository.GetSavedPostsAsync(userId);
			if (savedPosts.Count() == 0)
			{
				savedPosts = new List<Post>();
				return new ServiceResponse<List<Post>>() { Entities = null, Success = true, Message = "No Posts Saved" };
			}

			return new ServiceResponse<List<Post>>() { Entities = savedPosts, Success = true, Message = "Posts found" };
		}

		public async Task<ServiceResponse<Post>> UnsavePostAsync(Guid postId, string userId)
		{
			var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
			if (post == null)
				return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "Post not found!" };

			var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
			if (user == null)
				return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "User not found!" };

			await _unitOfWork.SavedPostRepository.UnsavePostAsync(post.ID, user.Id);
			return new ServiceResponse<Post>() { Entities = null, Success = true, Message = "Post Unsaved." };
		}

		public void Dett(Post post)
		{
			_unitOfWork.PostRepository.Detach(post);
		}

		// Private
		private async Task<string> SaveImage(byte[] imageBytes, IHostingEnvironment hostEnvironment)
		{
			// Ensure that the image is not null
			if (imageBytes == null || imageBytes.Length == 0)
			{
				throw new ArgumentException("Image cannot be null or empty.");
			}

			// Generate a unique filename for the image
			string fileName = $"{Guid.NewGuid()}.jpg";

			// Combine the content root path and an "Uploads" directory to store the images
			string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Uploads");

			// Ensure the "Uploads" directory exists; create it if not
			if (!Directory.Exists(uploadsFolder))
			{
				Directory.CreateDirectory(uploadsFolder);
			}

			// Combine the path to the uploads directory with the unique filename
			string filePath = Path.Combine(uploadsFolder, fileName);

			// Write the image bytes to the file
			await File.WriteAllBytesAsync(filePath, imageBytes);

			// Return the relative path to the saved image (for example, "Uploads/filename.jpg")
			return Path.Combine(filePath);
		}

		private async Task DeleteImage(string imageUrl, IHostingEnvironment hostEnvironment)
		{
			// Ensure that the image URL is not null or empty
			if (string.IsNullOrEmpty(imageUrl))
			{
				return;
			}

			// Combine the wwwroot path with the relative image path to get the full file path
			string imagePath = Path.Combine(hostEnvironment.WebRootPath, imageUrl);

			// Check if the file exists before attempting to delete it
			if (File.Exists(imagePath))
			{
				// Delete the image file
				File.Delete(imagePath);
			}
		}
	}
}
