using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.File
{
    public class FileService : IFileService
    {
        public async Task<ObjectResult> DownloadFile(string fileName)
        {
            try
            {
                // Define the file path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName);

                // Check if the file exists
                if (!System.IO.File.Exists(filePath))
                {
                    return new NotFoundObjectResult("File not found.");
                }

                // Read the file content
                var fileBytes = System.IO.File.ReadAllBytes(filePath);

                // Return the file as a downloadable content
                return new OkObjectResult(new { FileBytes = fileBytes, FileName = fileName });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { Message = "Internal server error", Details = ex.Message })
                {
                    StatusCode = 500
                };
            }
        }

        public async Task<ObjectResult> UploadFile(IFormFile file)
        {
            try
            {
                // Define the path to save the file
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

                // Ensure the directory exists
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Create a unique file name
                var filePath = Path.Combine(uploadPath, file.FileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new OkObjectResult(new { Message = "File uploaded successfully!", FileName = file.FileName });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { Message = "Internal server error", Details = ex.Message })
                {
                    StatusCode = 500
                };
            }


        }
    }
}
