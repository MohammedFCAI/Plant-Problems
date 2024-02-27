using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Plant_Problems.Data.Models;
using Plant_Problems.Infrastructure.Interfaces;
using Plant_Problems.Service.Interfaces;

namespace Plant_Problems.Service.Implementations
{
	public class ImagePredicationService : IImagePredicationService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHostingEnvironment _hostEnvironment;
		public ImagePredicationService(IUnitOfWork unitOfWork, IHostingEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
		}

		public async Task<ServiceResponse<ImagePredication>> AddImage(ImagePredication imagePredication)
		{
			try
			{
				// Save the image to the server
				string imagePath = await SaveImage(imagePredication.Image, _hostEnvironment);

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
				await DeleteImage(imagePredication.ImageUrl, _hostEnvironment);

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
			string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Model\\Uploads");

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
