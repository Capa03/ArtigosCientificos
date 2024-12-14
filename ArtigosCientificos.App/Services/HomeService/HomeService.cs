using ArtigosCientificos.App.Models.User;
using ArtigosCientificos.App.Services.ApiService;

namespace ArtigosCientificos.App.Services.HomeService
{
    public class HomeService : IHomeService
    {
        private readonly ApiService.ApiService _apiService;
        private readonly ConfigServer _configServer;
        public HomeService(ApiService.ApiService apiService, ConfigServer configServer = null)
        {
            this._apiService = apiService;
            _configServer = configServer;
        }

        public async Task<List<User>> GetUsers()
        {

            List<User> users = await this._apiService.GetTAsync<List<User>>(this._configServer.GetUsersUrl());
            foreach (var user in users)
            {
                Console.WriteLine(user.Username);
            }
            return users;
        }
    }
}
