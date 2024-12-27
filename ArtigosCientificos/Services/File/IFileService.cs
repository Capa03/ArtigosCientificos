using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.File
{
    /// <summary>
    /// Interface defining the contract for the file service operations.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Uploads a file to the server.
        /// </summary>
        /// <param name="file">The file to be uploaded.</param>
        /// <returns>An <see cref="ObjectResult"/> containing the result of the file upload.</returns>
        Task<ObjectResult> UploadFile(IFormFile file);

        /// <summary>
        /// Downloads a file from the server by its filename.
        /// </summary>
        /// <param name="fileName">The name of the file to be downloaded.</param>
        /// <returns>An <see cref="ObjectResult"/> containing the requested file.</returns>
        Task<ObjectResult> DownloadFile(string fileName);
    }
}
