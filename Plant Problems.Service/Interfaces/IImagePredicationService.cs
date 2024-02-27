using Plant_Problems.Data.Models;

namespace Plant_Problems.Service.Interfaces
{
	public interface IImagePredicationService
	{
		Task<ServiceResponse<ImagePredication>> AddImage(ImagePredication imagePredication);

		//Task<ServiceResponse<ImagePredication>> DeleteImagePrdications(List<ImagePredication> imagePredicationList);
		Task<ServiceResponse<List<ImagePredication>>> DeleteImagePrdications(string userId);

		Task<ServiceResponse<List<ImagePredication>>> GetImagePricationsByUserId(string userId);
	}
}
