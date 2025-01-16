using System.Net;
using ArtigosCientificos.Api.Models.Article;
using ArtigosCientificos.Api.Services.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtigosCientificos.Api.Models.User;
using ArtigosCientificos.Api.Services.Author;
using ArtigosCientificos.Api.Services.AuthService;

namespace ArtigosCientificos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService, IAuthorService authorService)
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

        [HttpGet("users")]
        //[Authorize]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {

            ObjectResult objectResult = await _articleService.GetUsers();

            if (objectResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(objectResult.Value);
            }

            return Ok(objectResult.Value);
        }

        /*[HttpGet("articles")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            ObjectResult article = await _articleService.GetArticleB();

            if (articles.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(articles.Value);
            }

            return Ok(articles.Value);
        }
        */
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
    }
}
