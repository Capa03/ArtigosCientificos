
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
        /// API call to login
        /// </summary>
        /// <param name="userDTO">The user data transfer object containing login credentials</param>
        /// <returns>A valid LoginRequest object if the login is successful</returns>
        /// <exception cref="InvalidOperationException">Thrown when loginRequest is null or contains an error status code</exception>
        public async Task<LoginRequest> Login(UserDTO userDTO)
        {
            LoginRequest? loginRequest = await this._apiService.PostAsync<LoginRequest>(this._configServer.GetLoginUrl(), userDTO);

            if (loginRequest == null)
            {
                throw new InvalidOperationException("Login request failed. The server returned a null response.");
            }


            return loginRequest;
        }

    }
}
