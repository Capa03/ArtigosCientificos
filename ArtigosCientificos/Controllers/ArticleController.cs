using System.Net;
using ArtigosCientificos.Api.Models.Article;
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

        [HttpGet("articles/last")]
        public async Task<IActionResult> GetLastArticle()
        {
            ObjectResult article = await _articleService.GetLastArticle();
            if (article.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(article.Value);
            }
            return Ok(article.Value);
        }
    }
}
