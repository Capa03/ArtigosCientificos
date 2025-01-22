using System.Net;
using ArtigosCientificos.Api.Models.Article;
using ArtigosCientificos.Api.Models.Category;
using ArtigosCientificos.Api.Services.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("articles")]
        public async Task<IActionResult> GetAllArticles()
        {
            ObjectResult articles = await _articleService.GetAllArticles();

            if (articles.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(articles.Value);
            }

            return Ok(articles.Value);
        }

        [HttpGet("articles/{id}")]
        public async Task<IActionResult> GetArticle(int id)
        {
            // Call the service method and get the article
            var article = await _articleService.GetArticlebyId(id);

            if (article == null)
            {
                // If article is not found, return 404 Not Found
                return NotFound("Article not found");
            }

            // If article is found, return it with a 200 OK status
            return Ok(article);
        }



        [HttpPost("articles")]
        [Authorize(Roles = "Researcher")]
        public async Task<IActionResult> CreateArticle(ArticleDTO article)
        {
            ObjectResult result = await _articleService.CreateArticle(article);
            if (result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                return BadRequest(result.Value);
            }
            return Ok(result.Value);
        }


        [HttpGet("articles/accepted")]
        public async Task<IActionResult> GetAcceptedArticles()
        {
            ObjectResult articles = await _articleService.GetAcceptedArticles();
            if (articles.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(articles.Value);
            }
            return Ok(articles.Value);
        }

        [HttpGet("articles/categories")]
        public async Task<IActionResult> GetCategories()
        {
            ObjectResult categories = await _articleService.GetCategories();
            if (categories.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(categories.Value);
            }
            return Ok(categories.Value);
        }

        [HttpGet("articles/categories/{id}")]

        public async Task<IActionResult> GetCategorybyId(int id)
        {
            Category category = await _articleService.GetCategorybyId(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }
            return Ok(category);
        }

        [HttpPut("article/downloadsCounter/{id}")]
        public async Task<IActionResult> IncrementDownloadsCounter(int id)
        {
            Article result = await _articleService.IncrementDownloadsCounter(id);
            if (result == null)
            {
                // If article is not found, return 404 Not Found
                return NotFound("Article not found");
            }

            // If article is found, return it with a 200 OK status
            return Ok(result);
        }
    }
}
