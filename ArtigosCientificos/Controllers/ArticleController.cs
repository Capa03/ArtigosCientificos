using System.Net;
using ArtigosCientificos.Api.Models.Article;
using ArtigosCientificos.Api.Services.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ArtigosCientificos.Api.Models.User;
using ArtigosCientificos.Api.Services.Author;
using ArtigosCientificos.Api.Services.AuthService;
using ArtigosCientificos.Api.Models.Category;

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

        [HttpGet("categories")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            ObjectResult objectResult = await _articleService.GetCategories();

            if (objectResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(objectResult.Value);
            }

            return Ok(objectResult.Value);
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

        [HttpGet("search/")]
        public async Task<IActionResult> GetAcceptedArticlesFiltered(ArticleFilteredDTO articleFilteredDTO)
        {
            ObjectResult objectResult = await _articleService.GetAcceptedArticlesFiltered(articleFilteredDTO);

            if (objectResult.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(objectResult.Value);
            }

            return Ok(objectResult.Value);
        }

        [HttpGet("search/{searchString}")]
        public async Task<IActionResult> GetApcceptedArticleByString(string searchString)
        {
            var result = await _articleService.GetArticlesBySearch(searchString);

            if (result is NotFoundObjectResult notFound)
            {
                return NotFound("Review not found.");
            }

            return Ok(result.Value);
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

        [HttpGet("articles/search")]
        public async Task<IActionResult> Search()
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
