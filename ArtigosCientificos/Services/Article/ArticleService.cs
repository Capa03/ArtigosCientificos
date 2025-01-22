
using ArtigosCientificos.Api.Data;
using ArtigosCientificos.Api.Models.Article;
using ArtigosCientificos.Api.Models.Category;
using ArtigosCientificos.Api.Models.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
                CategoryId = articleDTO.CategoryId
            };
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



        public async Task<ObjectResult> GetAllArticles()
        {
            List<Article> articles = _context.Articles.ToList();

            if (articles.Count == 0)
            {
                return new NotFoundObjectResult("No articles found");
            }

            return new OkObjectResult(articles);
        }

        public async Task<Article> GetArticlebyId(int id)
        {
            Article article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return null;
            }

            article.Views = article.Views + 1;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();

            return article;
        }

        public async Task<Article> IncrementDownloadsCounter(int id)
        {
            Article article = await _context.Articles.FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return null;
            }

            article.Downloads = article.Downloads + 1;
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return article;
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
