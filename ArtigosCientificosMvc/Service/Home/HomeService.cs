using ArtigosCientificosMvc.Models.Article;
using ArtigosCientificosMvc.Models.User;
using ArtigosCientificosMvc.Service.Api;
using Microsoft.Extensions.Logging;

namespace ArtigosCientificosMvc.Service.Home
{
    public class HomeService : IHomeService
    {
        private readonly ConfigServer _configServer;
        private readonly ApiService _apiService;
        private readonly ILogger<HomeService> _logger; 

        public HomeService(ConfigServer configServer, ApiService apiService, ILogger<HomeService> logger)
        {
            _configServer = configServer;
            _apiService = apiService;
            _logger = logger; 
        }

        public async Task<List<Article>> getArticles()
        {
            try
            {
                List<Article> articles = await _apiService.GetTAsync<List<Article>>(_configServer.GetAcceptedArticlesUrl());

                if (articles == null || !articles.Any())
                {
                    _logger.LogWarning("No articles found.");
                    return new List<Article>();
                }

                return articles;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while fetching articles.");

                return new List<Article>(); 
            }
        }

        public async Task<List<User>> getUsers()
        {
            try
            {
                List<User> users = await _apiService.GetTAsync<List<User>>(_configServer.GetUsersArticleUrl());

                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No articles found.");
                    return new List<User>();
                }

                return users;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while fetching articles.");

                return new List<User>();
            }
        }

        /*public async Task<Article> getArticle(int id)
        {
            try
            {
                Article article = await _apiService.GetTAsync<Article>(_configServer.GetArticleByIdUrl(id));

                if (article == null)
                {
                    _logger.LogWarning("No article found.");
                    return new Article();
                }

                return article;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occurred while fetching articles.");

                return new Article();
            }
        }*/
    }
}
