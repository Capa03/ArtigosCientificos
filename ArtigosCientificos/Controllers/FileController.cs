using ArtigosCientificos.Api.Services.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;

[ApiController]
[Route("api/[controller]")]
public class FileController : Controller
{
    private readonly IFileService _fileService;

    private readonly IHttpClientFactory _httpClientFactory;

    public FileController(IFileService fileService, IHttpClientFactory httpClientFactory)
    {
        this._fileService = fileService;
        _httpClientFactory = httpClientFactory;
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
        var sanitizedFileName = Path.GetFileName(fileName); 
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

    [HttpGet]
    [Route("showPdf/{fileName}")]
    public async Task<IActionResult> ShowPdf(string fileName)
    {
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
        var filePath = Path.Combine(uploadPath, fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("The requested file does not exist.");
        }

        var memoryStream = new MemoryStream(await System.IO.File.ReadAllBytesAsync(filePath));

        // Set the inline Content-Disposition to open in the browser
        Response.Headers.Append("Content-Disposition", $"inline; filename={fileName}");
        return new FileStreamResult(memoryStream, "application/pdf");
    }
}
