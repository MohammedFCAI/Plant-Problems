using Microsoft.AspNetCore.Http;

namespace Plant_Problems.Service.Implementations
{
    public class ImageManager : IImageManager
    {

        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _accessor;

        public ImageManager(IWebHostEnvironment environment, IHttpContextAccessor accessor)
        {
            _environment = environment;
            _accessor = accessor;
        }

        public async Task<string> SaveImage(byte[] imageBytes, IHostingEnvironment hostEnvironment, string type)
        {
            var imageUrl = string.Empty;

            // Ensure that the image is not null

            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentException("Image cannot be null or empty.");
            }

            // Generate a unique filename for the image
            string fileName = $"{Guid.NewGuid()}.jpg";

            // Combine the content root path and an "Uploads" directory to store the images

            var context = _accessor.HttpContext!.Request;
            var baseUrl = context.Scheme + "://" + context.Host; // htttps://localhost:3030


            string uploadsFolder;
            if (type == "MODEL")
            {
                imageUrl = $"{baseUrl}/Model/Uploads/{fileName}";
                uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Model\\Uploads");
            }

            else
            {
                imageUrl = $"{baseUrl}/Uploads/{fileName}";

                uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Uploads");
            }


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
            //return Path.Combine(filePath);
            return imageUrl;
        }

        public async Task DeleteImage(string imageUrl, IHostingEnvironment hostEnvironment, string type)
        {
            var imageNewUrl = string.Empty;
            // Ensure that the image URL is not null or empty
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }
            // E://Mohamed//Testproject//Uploads/dferidfdfdfdi.jpg
            // "imageUrl": "https://localhost:7062/Uploads/3bc44e80-470a-4805-b419-80302c6c99d9.jpg"

            // Combine the wwwroot path with the relative image path to get the full file path

            var fileName = Path.GetFileName(imageUrl);
            if (type == "MODEL")
                imageNewUrl = _environment.WebRootPath + "\\Model\\Uploads\\" + fileName;
            else
                imageNewUrl = _environment.WebRootPath + "\\Uploads\\" + fileName;

            //string imagePath = Path.Combine(hostEnvironment.WebRootPath, imageUrl);

            // Check if the file exists before attempting to delete it
            if (File.Exists(imageNewUrl))
            {
                // Delete the image file
                File.Delete(imageNewUrl);
            }
        }
    }
}
