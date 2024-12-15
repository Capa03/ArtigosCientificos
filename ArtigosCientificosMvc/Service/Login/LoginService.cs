using System.Net;
using ArtigosCientificosMvc.Models.Login;
using ArtigosCientificosMvc.Service.Api;
using Microsoft.AspNetCore.Http;

namespace ArtigosCientificosMvc.Service.Login
{
    public class LoginService : ILoginService
    {
        private readonly ConfigServer _configServer;
        private readonly ApiService _apiService;

        public LoginService(ConfigServer configServer, ApiService apiService)
        {
            this._configServer = configServer;
            this._apiService = apiService;

        }

        public async Task<LoginRequest> Login(UserDTO userDTO)
        {
            try
            {
                var (loginRequest, statusCode) = await this._apiService.PostAsync<LoginRequest>(this._configServer.GetLoginUrl(), userDTO);

                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    throw new HttpRequestException("Invalid login credentials. Please check your username and password.");
                }
                else if (statusCode == HttpStatusCode.BadRequest)
                {
                    throw new HttpRequestException("Bad request. Please ensure all required fields are filled correctly.");
                }
                else if (loginRequest == null)
                {
                    throw new InvalidOperationException("Login request failed. The server returned a null or empty response.");
                }

                return loginRequest;
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("An unexpected error occurred during login.", ex);
            }
        }
    }
}
