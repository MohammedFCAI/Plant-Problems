namespace Plant_Problems.Service.Implementations
{
    public class ImagePredicationService : IImagePredicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IImageManager _imageManager;

        public ImagePredicationService(IUnitOfWork unitOfWork, IHostingEnvironment hostEnvironment, IImageManager imageManager)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _imageManager = imageManager;
        }

        public async Task<ServiceResponse<ImagePredication>> AddImage(ImagePredication imagePredication)
        {
            try
            {
                // Save the image to the server
                //string imagePath = await SaveImage(imagePredication.Image, _hostEnvironment);
                string imagePath = await _imageManager.SaveImage(imagePredication.Image, _hostEnvironment, "MODEL");


                // Update the ImageUrl property with the path where the image is stored
                imagePredication.ImageUrl = imagePath;
                imagePredication.CreatedOn = DateTime.Now;

                // Save the post to the database
                await _unitOfWork.ImagePredicationRepositry.AddAsync(imagePredication);

                // Return a success response with the updated post
                return new ServiceResponse<ImagePredication>() { Entities = imagePredication, Success = true, Message = "Post added successfully" };
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the process
                return new ServiceResponse<ImagePredication>() { Entities = null, Success = false, Message = $"Error adding post: {ex.Message}" };
            }
        }

        public async Task<ServiceResponse<List<ImagePredication>>> DeleteImagePrdications(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return new ServiceResponse<List<ImagePredication>>() { Entities = null, Success = false, Message = "User is not found!" };

            var imagesPredications = await _unitOfWork.ImagePredicationRepositry.GetImagePredicationsByUserIdAsync(userId);

            if (imagesPredications.IsNullOrEmpty())
                return new ServiceResponse<List<ImagePredication>>() { Entities = null, Success = false, Message = "Imags empty!" };

            foreach (var imagePredication in imagesPredications)
                await _imageManager.DeleteImage(imagePredication.ImageUrl, _hostEnvironment);


            await _unitOfWork.ImagePredicationRepositry.DeleteImagesAsync(imagesPredications);
            return new ServiceResponse<List<ImagePredication>>() { Entities = imagesPredications, Success = true, Message = "Images deleted" };

        }

        public async Task<ServiceResponse<List<ImagePredication>>> GetImagePricationsByUserId(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return new ServiceResponse<List<ImagePredication>>() { Entities = null, Success = false, Message = "User is not found!" };

            var imagesPredications = await _unitOfWork.ImagePredicationRepositry.GetImagePredicationsByUserIdAsync(userId);

            if (imagesPredications.IsNullOrEmpty())
            {
                imagesPredications = new List<ImagePredication>();
                return new ServiceResponse<List<ImagePredication>>() { Entities = imagesPredications, Success = true, Message = "Imags empty!" };
            }

            return new ServiceResponse<List<ImagePredication>>() { Entities = imagesPredications, Success = true, Message = "Images found" };
        }


    }
}
