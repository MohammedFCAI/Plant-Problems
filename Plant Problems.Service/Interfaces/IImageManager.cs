namespace Plant_Problems.Service.Interfaces
{
    public interface IImageManager
    {
        Task<string> SaveImage(byte[] imageBytes, IHostingEnvironment hostEnvironment, string type);
        Task DeleteImage(string imageUrl, IHostingEnvironment hostEnvironment);
    }
}
