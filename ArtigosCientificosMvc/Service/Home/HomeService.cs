using ArtigosCientificosMvc.Models.Article;
using ArtigosCientificosMvc.Models.Category;
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

        public async Task<Article> GetArticle(int id)
        {
            Article article = await _apiService.GetTAsync<Article>(_configServer.GetArticlesByIdUrl(id));

            if (article == null)
            {
                _logger.LogWarning("No article found.");
                return new Article();
            }

            return article;
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

        public async Task<User> GetUser(int id)
        {
            User user = await _apiService.GetTAsync<User>(_configServer.GetUserById(id));

            if (user == null) {

                _logger.LogWarning("No user found.");
                return new User();
            }
            return user;
        }

        public async Task<Article> IncrementDownloadsCounter(int id)
        {
            Article article = await _apiService.GetTAsync<Article>(_configServer.GetUserById(id));

            if (article == null)
            {

                _logger.LogWarning("No article found.");
                return new Article();
            }
            return article;
        }

        public async Task<List<Category>> GetCategories()
        {
            List<Category> data = await _apiService.GetTAsync<List<Category>>(this._configServer.GetCategoriesUrl());
            return data;
        }

        public async Task<ArticleStatistics> GetArticleStatistics()
        {
            var articles = await getArticles();
            var totalViews = articles.Sum(a => a.Views ?? 0);
            var totalDownloads = articles.Sum(a => a.Downloads ?? 0);
            var categoryGroups = articles.GroupBy(a => a.CategoryId)
                                          .Select(g => new { CategoryId = g.Key, ViewCount = g.Sum(a => a.Views ?? 0) })
                                          .OrderByDescending(g => g.ViewCount)
                                          .FirstOrDefault();

            var mostViewedCategory = categoryGroups != null ? await GetCategoryName(categoryGroups.CategoryId) : "None";

            return new ArticleStatistics
            {
                TotalArticles = articles.Count,
                TotalViews = totalViews,
                TotalDownloads = totalDownloads,
                MostViewedCategory = mostViewedCategory,
                MostViewedCategoryCount = categoryGroups?.ViewCount ?? 0
            };
        }

        private async Task<string> GetCategoryName(int categoryId)
        {
            var category = await GetCategory(categoryId);  
            return category?.Name ?? "Unknown";
        }

        private async Task<Category> GetCategory(int categoryId)
        {
            Category data = await _apiService.GetTAsync<Category>(this._configServer.GetCategoriesByIdUrl(categoryId));
            return data;
        }
    }
}
