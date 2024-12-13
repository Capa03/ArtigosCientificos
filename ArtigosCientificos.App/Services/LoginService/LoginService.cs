
using System.Net;
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
        /// API call to log in a user.
        /// </summary>
        /// <param name="userDTO">The user data transfer object containing login credentials.</param>
        /// <returns>A valid LoginRequest object if the login is successful.</returns>
        /// <exception cref="HttpRequestException">Thrown when the server response indicates a failure.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the response from the server is null or invalid.</exception>
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
