namespace Plant_Problems.Service.Implementations
{
    public class ImageManager : IImageManager
    {
        public async Task<string> SaveImage(byte[] imageBytes, IHostingEnvironment hostEnvironment, string type)
        {
            // Ensure that the image is not null
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentException("Image cannot be null or empty.");
            }

            // Generate a unique filename for the image
            string fileName = $"{Guid.NewGuid()}.jpg";

            // Combine the content root path and an "Uploads" directory to store the images
            string uploadsFolder;
            if (type == "MODEL")
                uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Model\\Uploads");
            else
                uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "Uploads");


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

        public async Task DeleteImage(string imageUrl, IHostingEnvironment hostEnvironment)
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
