using System.Net;
using ArtigosCientificosMvc.Models.Login;
using ArtigosCientificosMvc.Service.Api;
using ArtigosCientificosMvc.Service.Token;
using Microsoft.AspNetCore.Http;

namespace ArtigosCientificosMvc.Service.Login
{
    public class LoginService : ILoginService
    {
        private readonly ConfigServer _configServer;
        private readonly ApiService _apiService;
        private readonly TokenManager _tokenManager;
        public LoginService(ConfigServer configServer, ApiService apiService, TokenManager tokenManager)
        {
            this._configServer = configServer;
            this._apiService = apiService;
            this._tokenManager = tokenManager;
        }

        public async Task<LoginResult> Login(UserDTO userDTO)
        {
            try
            {
                var (loginRequest, statusCode) = await this._apiService.PostAsync<LoginRequest>(this._configServer.GetLoginUrl(), userDTO);

                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    return new LoginResult { Success = false, Message = "Invalid login credentials. Please check your username and password." };
                }
                else if (statusCode == HttpStatusCode.BadRequest)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Bad request. Please ensure all required fields are filled correctly."
                    };
                }
                else if (loginRequest == null)
                {
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login request failed. The server returned a null or empty response."
                    };
                }

                await this._tokenManager.SetTokenAsync(loginRequest.Value);

                return new LoginResult
                {
                    Success = true,
                    Message = "User registered successfully."
                };
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException("An unexpected error occurred during login.", ex);
            }
        }
    }
}
