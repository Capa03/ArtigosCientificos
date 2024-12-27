using ArtigosCientificosMvc.Models.Article;

namespace ArtigosCientificosMvc.Service.Home
{
    public interface IHomeService
    {
        Task<List<Article>> getArticles();
    }
}
