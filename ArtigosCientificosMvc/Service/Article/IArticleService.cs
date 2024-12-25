
using ArtigosCientificosMvc.Models.Article;

namespace ArtigosCientificosMvc.Service.Articles
{
    public interface IArticleService
    {
        Task<ArticleResult> Create(Article article);
    }
}
