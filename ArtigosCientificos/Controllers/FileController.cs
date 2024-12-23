using ArtigosCientificos.Api.Services.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[Route("api/[controller]")]
public class FileController : Controller
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        this._fileService = fileService;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "Researcher")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty.");
        }

        if (file.ContentType != "application/pdf")
        {
            return BadRequest("Only PDF files are allowed.");
        }

        ObjectResult objectResult = await _fileService.UploadFile(file);

        if (objectResult.StatusCode == (int)HttpStatusCode.BadRequest)
        {
            return BadRequest(objectResult.Value);
        }

        return Ok(objectResult.Value);
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        var sanitizedFileName = Path.GetFileName(fileName); // Prevent directory traversal
        ObjectResult objectResult = await _fileService.DownloadFile(sanitizedFileName);

        if (objectResult.StatusCode == (int)HttpStatusCode.NotFound)
        {
            return NotFound(objectResult.Value);
        }

        var result = objectResult.Value as dynamic;
        byte[] fileBytes = result.FileBytes;
        string downloadFileName = result.FileName;

        return File(fileBytes, "application/pdf", downloadFileName);
    }
}
