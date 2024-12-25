using System.Net;
using ArtigosCientificosMvc.Models.Article;
using ArtigosCientificosMvc.Service.Api;
using ArtigosCientificosMvc.Service.Token;

namespace ArtigosCientificosMvc.Service.Articles
{
    public class ArticleService : IArticleService
    {
        private readonly ApiService apiService;
        private readonly TokenManager tokenManager;
        private readonly ConfigServer configServer;
        public ArticleService(ApiService apiService, TokenManager tokenManager, ConfigServer configServer)
        {
            this.apiService = apiService;
            this.tokenManager = tokenManager;
            this.configServer = configServer;
        }

        public async Task<ArticleResult> Create(Article article)
        {
            int UserId = await tokenManager.GetUserId();
            article.UserId = UserId;
            Console.WriteLine("ArticleService.Create: article Title " + article.Title);
            Console.WriteLine("ArticleService.Create: article Abstract " + article.Abstract);
            Console.WriteLine("ArticleService.Create: article Keywords " + article.Keywords);
            Console.WriteLine("ArticleService.Create: article UserId " + article.UserId);
            Console.WriteLine("ArticleService.Create: article File " + article.File);
            Console.WriteLine("ArticleService.Create: article Status " + article.Status);
            try
            {
                var (data, statusCode) = await apiService.PostAsync<Article>(this.configServer.GetArticlesCreateUrl(), article);
                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    return new ArticleResult
                    {
                        Success = false,
                        Message = "Unauthorized"
                    };
                }

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return new ArticleResult
                    {
                        Success = false,
                        Message = "Not Found"
                    };
                }

                if (statusCode == HttpStatusCode.BadRequest)
                {
                    return new ArticleResult
                    {
                        Success = false,
                        Message = "Bad Request"
                    };
                }

                return new ArticleResult
                {
                    Success = true,
                    Message = "Article created successfully"
                };
            }
            catch (Exception e)
            {
                return new ArticleResult
                {
                    Success = false,
                    Message = e.Message
                };
            }

        }
    }
}
