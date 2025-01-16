using ArtigosCientificosMvc.Models.Article;
using ArtigosCientificosMvc.Models.User;

namespace ArtigosCientificosMvc.Service.Home
{
    /// <summary>
    /// Defines methods for retrieving articles in the home page context.
    /// </summary>
    public interface IHomeService
    {
        /// <summary>
        /// Asynchronously retrieves a list of articles.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result is a <see cref="List{Article}"/> containing the retrieved articles.</returns>
        Task<List<Article>> getArticles();

        Task<List<User>> getUsers();
    }
}
