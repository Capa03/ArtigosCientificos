using System.Net;
using ArtigosCientificosMvc.Models.Login;
using ArtigosCientificosMvc.Service.Api;
using ArtigosCientificosMvc.Service.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ArtigosCientificosMvc.Service.Login
{
    public class LoginService : ILoginService
    {
        private readonly ConfigServer _configServer;
        private readonly ApiService _apiService;
        private readonly TokenManager _tokenManager;
        private readonly ILogger<LoginService> _logger;

        public LoginService(ConfigServer configServer, ApiService apiService, TokenManager tokenManager, ILogger<LoginService> logger)
        {
            _configServer = configServer;
            _apiService = apiService;
            _tokenManager = tokenManager;
            _logger = logger;
        }

        public async Task<LoginResult> Login(UserDTO userDTO)
        {
            try
            {
                var (token, statusCode) = await _apiService.PostAsync<string>(_configServer.GetLoginUrl(), userDTO);

                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogWarning("Unauthorized login attempt with username: {Username}", userDTO.Username);
                    return new LoginResult { Success = false, Message = "Invalid login credentials. Please check your username and password." };
                }
                else if (statusCode == HttpStatusCode.BadRequest)
                {
                    _logger.LogWarning("Bad request during login with username: {Username}", userDTO.Username);
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Bad request. Please ensure all required fields are filled correctly."
                    };
                }
                else if (token == null)
                {
                    _logger.LogError("Login request failed for username: {Username}, received null response.", userDTO.Username);
                    return new LoginResult
                    {
                        Success = false,
                        Message = "Login request failed. The server returned a null or empty response."
                    };
                }

                _logger.LogInformation("User logged in successfully. Token: {Token}", token);

                
                await _tokenManager.SetTokenAsync(token);

                return new LoginResult
                {
                    Success = true,
                    Message = "User logged in successfully."
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred during the login request.");
                throw new ApplicationException("An unexpected error occurred during login.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during the login process.");
                throw;
            }
        }
    }
}
