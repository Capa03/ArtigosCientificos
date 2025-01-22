using ArtigosCientificos.Api.Models.Article;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.Articles
{
    /// <summary>
    /// Interface defining the contract for article-related services, such as retrieving, creating, and managing articles.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Retrieves a list of all articles in the system.
        /// </summary>
        /// <returns>An <see cref="ObjectResult"/> containing a list of all articles.</returns>
        Task<ObjectResult> GetAllArticles();

        /// <summary>
        /// Retrieves a new specific Article in the system.
        /// </summary>
        /// <param name="article">The article details to be created.</param>
        /// <returns>An <see cref="ObjectResult"/> indicating the outcome of the creation operation.</returns>
        //Task<ObjectResult> GetArticles(int id);

        /// <summary>
        /// Creates a new article in the system.
        /// </summary>
        /// <param name="article">The article details to be created.</param>
        /// <returns>An <see cref="ObjectResult"/> indicating the outcome of the creation operation.</returns>
        Task<ObjectResult> CreateArticle(ArticleDTO article);

        /// <summary>
        /// Retrieves a list of accepted articles in the system.
        /// </summary>
        /// <returns>An <see cref="ObjectResult"/> containing a list of accepted articles.</returns>
        Task<ObjectResult> GetAcceptedArticles();

        Task<ObjectResult> GetCategories();
    }
}
