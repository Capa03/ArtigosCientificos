using ArtigosCientificosMvc.Models.Article;
using ArtigosCientificosMvc.Models.Category;

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

        /// <summary>
        /// Gets all Categorys.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Category>> GetCategories();
    }
}
