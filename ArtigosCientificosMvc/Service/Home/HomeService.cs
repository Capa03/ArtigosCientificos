
using ArtigosCientificosMvc.Models.User;
using ArtigosCientificosMvc.Service.Api;

namespace ArtigosCientificosMvc.Service.Home
{
    public class HomeService : IHomeService
    {

        private readonly ConfigServer _configServer;
        private readonly ApiService _apiService;
        public HomeService(ConfigServer configServer, ApiService apiService)
        {
            _configServer = configServer;
            _apiService = apiService;
        }

        public async Task<List<User>> GetUsers()
        {
            return await this._apiService.GetTAsync<List<User>>(this._configServer.GetUsersUrl());
        }
    }
}
