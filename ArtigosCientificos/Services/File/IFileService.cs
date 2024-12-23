using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.File
{
    public interface IFileService
    {
        Task<ObjectResult> UploadFile(IFormFile file);

        Task<ObjectResult> DownloadFile(string fileName);
    }
}
