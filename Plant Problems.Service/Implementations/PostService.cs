namespace Plant_Problems.Service.Implementations
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IImageManager _imageManager;

        public PostService(IUnitOfWork unitOfWork, IHostingEnvironment hostEnvironment, IImageManager imageManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _imageManager = imageManager;
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
                    //string imagePath = await SaveImage(post.Image, _hostEnvironment);
                    string imagePath = await _imageManager.SaveImage(post.Image, _hostEnvironment, "DEFAULT");

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

            await _imageManager.DeleteImage(post.ImageUrl, _hostEnvironment);

            if (entity.Image != null)
            {
                try
                {
                    // Save the image to the server
                    string imagePath = await _imageManager.SaveImage(entity.Image, _hostEnvironment, "DEFAULT");

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

            await _imageManager.DeleteImage(post.ImageUrl, _hostEnvironment);
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

            var isSavedBefore = await _unitOfWork.SavedPostRepository.IsSaved(post, user);

            if (!isSavedBefore)
                return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "Post alrady saved before." };

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

            var isNotSavedBefore = !await _unitOfWork.SavedPostRepository.IsSaved(post, user);
            if (!isNotSavedBefore)
                return new ServiceResponse<Post>() { Entities = null, Success = false, Message = "No Posts Saved." };

            await _unitOfWork.SavedPostRepository.UnsavePostAsync(post.ID, user.Id);
            return new ServiceResponse<Post>() { Entities = null, Success = true, Message = "Post Unsaved." };
        }

        public void Dett(Post post)
        {
            _unitOfWork.PostRepository.Detach(post);
        }

    }
}
