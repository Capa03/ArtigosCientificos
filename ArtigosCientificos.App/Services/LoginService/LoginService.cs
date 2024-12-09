
using ArtigosCientificos.App.Models.Login;
using ArtigosCientificos.App.Models.User;


namespace ArtigosCientificos.App.Services.LoginService
{
    public class LoginService : ILoginService
    {

        private readonly ConfigServer _configServer;
        private readonly ApiService.ApiService _apiService;
        public LoginService(ApiService.ApiService apiService)
        {
            this._configServer = new ConfigServer();
            this._apiService = apiService;
        }

        /// <summary>
        /// Api call to login
        /// </summary>
        /// <param name="userDTO"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<LoginRequest> Login(UserDTO userDTO)
        {
            
            return await this._apiService.PostAsync<LoginRequest>(this._configServer.GetLoginUrl(), userDTO);
        }
    }
}
