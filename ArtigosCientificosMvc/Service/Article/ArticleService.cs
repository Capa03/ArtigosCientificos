using System.Net;
using ArtigosCientificosMvc.Models.Article;
using ArtigosCientificosMvc.Models.Category;
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
            int UserId = await tokenManager.GetUserIdAsync();
            article.UserId = UserId;
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

        public async Task<List<Category>> GetCategories()
        {
            return await apiService.GetTAsync<List<Category>>(this.configServer.GetCategoriesUrl());
        }
    }
}
