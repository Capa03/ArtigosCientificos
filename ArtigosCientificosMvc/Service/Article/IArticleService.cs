using ArtigosCientificosMvc.Models.Article;

namespace ArtigosCientificosMvc.Service.Articles
{
    /// <summary>
    /// Defines methods for managing articles, such as creating a new article.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Creates a new article.
        /// </summary>
        /// <param name="article">The article to be created.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result is an <see cref="ArticleResult"/> containing the result of the create operation.</returns>
        Task<ArticleResult> Create(Article article);
    }
}
