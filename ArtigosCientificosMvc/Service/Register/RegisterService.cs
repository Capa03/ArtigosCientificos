using System.Net;
using ArtigosCientificosMvc.Models.Login;
using ArtigosCientificosMvc.Service.Api;

namespace ArtigosCientificosMvc.Service.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly ApiService _apiService;
        private readonly ConfigServer _configServer;
        public RegisterService(ApiService apiService, ConfigServer configServer)
        {
            this._apiService = apiService;
            _configServer = configServer;
        }
        public async Task<string> Register(UserDTO registerDTO)
        {
            try
            {
                UserDTO userDto = new UserDTO
                {
                    Username = registerDTO.Username,
                    Password = registerDTO.Password,
                    Email = registerDTO.Email
                };

                var (user, statusCode) = await this._apiService.PostAsync<UserDTO>(this._configServer.GetRegisterUrl(), userDto);

                // Handle the status code
                if (statusCode == HttpStatusCode.BadRequest)
                {
                    return "User already exists.";
                }

                if (statusCode == HttpStatusCode.OK && user != null)
                {
                    return "User registered successfully.";
                }

                return "An unexpected error occurred.";
            }
            catch (HttpRequestException ex)
            {
                return "An error occurred while registering. Please try again later.";
            }
        }
    }
}
