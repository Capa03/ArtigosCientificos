using ArtigosCientificos.Api.Data;
using ArtigosCientificos.Api.Models.Article;
using ArtigosCientificos.Api.Models.Category;
using ArtigosCientificos.Api.Models.Review;
using ArtigosCientificos.Api.Models.User;
using Microsoft.AspNetCore.Mvc;


namespace ArtigosCientificos.Api.Services.Articles
{
    public class ArticleService : IArticleService
    {
        private DataContext _context;
        public ArticleService(DataContext context)
        {
            _context = context;
        }

        public async Task<ObjectResult> CreateArticle(ArticleDTO articleDTO)
        {

            Article article = new Article
            {
                Title = articleDTO.Title,
                Abstract = articleDTO.Abstract,
                Keywords = articleDTO.Keywords,
                File = articleDTO.File,
                UserId = articleDTO.UserId,
                CategoryId = articleDTO.CategoryID
            };

            Console.WriteLine(articleDTO.CategoryID + "SAM");

            var result = await _context.Articles.AddAsync(article);
            if (result == null)
            {
                return new BadRequestObjectResult("Error creating article");
            }
            await _context.SaveChangesAsync();

            Review review = new Review
            {
                ArticleId = article.Id,
                UserId = article.UserId,
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return new OkObjectResult(article);
        }

        public async Task<ObjectResult> GetArticlesBySearch(string searchString)
        {

            var articles = _context.Articles.Where(a => a.Title.Contains(searchString)).ToList();

            if (articles == null)
            {
                return new NotFoundObjectResult("No articles found with 'ACCEPTED' reviews.");
            }

            return new OkObjectResult(articles);
        }

        public async Task<ObjectResult> GetAcceptedArticles()
        {
            var articles = _context.Articles
                .Where(a => a.Reviews.Any(r => r.Status == "ACCEPTED"))
                .ToList();

            if (articles.Count == 0)
            {
                return new NotFoundObjectResult("No articles found with 'ACCEPTED' reviews.");
            }

            return new OkObjectResult(articles);
        }
        /*
        public async Task<List<Article>> GetAcceptedArticlesList()
        {
            var articles = _context.Articles
                .Where(a => a.Reviews.Any(r => r.Status == "ACCEPTED"))
                .ToList();

            if (articles.Count == 0)
            {
                return articles;
            }

            return articles;
        }*/

        public async Task<ObjectResult> GetAllArticles()
        {
            List<Article> articles = _context.Articles.ToList();

            if (articles.Count == 0)
            {
                return new NotFoundObjectResult("No articles found");
            }

            return new OkObjectResult(articles);
        }

        public async Task<ObjectResult> GetUsers()
        {
            List<User> users = _context.Users.ToList();

            if (users.Count == 0)
            {
                return new NotFoundObjectResult("No articles found");
            }

            return new OkObjectResult(users);
        }

        public async Task<ObjectResult> GetAcceptedArticlesFiltered(ArticleFilteredDTO articleFilteredDTO)
        {
            List<Article> articles = _context.Articles.Where(a => a.User.Username == articleFilteredDTO.Username 
            && a.Category.Name == articleFilteredDTO.Category).ToList();

            if (articles.Count == 0)
            {
                return new NotFoundObjectResult("No articles found");
            }

            return new OkObjectResult(articles);
        }

        public async Task<ObjectResult> GetCategories()
        {
            List<Category> categories = _context.Categories.ToList();

            if (categories.Count == 0)
            {
                return new NotFoundObjectResult("No categories found");
            }

            return new OkObjectResult(categories);
        }
    }
}
