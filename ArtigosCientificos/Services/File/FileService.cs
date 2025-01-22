using ArtigosCientificos.Api.Data;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.File
{
    public class FileService : IFileService
    {
        private readonly DataContext _context;
        public FileService() { }

        public async Task<ObjectResult> DownloadFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return new NotFoundObjectResult("File not found.");
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);

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
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var filePath = Path.Combine(uploadPath, file.FileName);

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
