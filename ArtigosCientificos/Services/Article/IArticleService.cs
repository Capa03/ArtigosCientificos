using ArtigosCientificos.Api.Models.Article;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.Articles
{
    public interface IArticleService
    {
        Task<ObjectResult> GetAllArticles();
        Task<ObjectResult> CreateArticle(ArticleDTO article);
        Task<ObjectResult> GetLastArticle();
    }
}
