using System.Net;
using ArtigosCientificosMvc.Models.Login;
using ArtigosCientificosMvc.Models.Register;
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
        public async Task<RegisterResult> Register(UserDTO registerDTO)
        {
            try
            {
                var userDto = new UserDTO
                {
                    Username = registerDTO.Username,
                    Password = registerDTO.Password,
                    Email = registerDTO.Email
                };

                var (user, statusCode) = await this._apiService.PostAsync<UserDTO>(this._configServer.GetRegisterUrl(), userDto);

                if (statusCode == HttpStatusCode.BadRequest)
                {
                    return new RegisterResult
                    {
                        Success = false,
                        Message = "User already exists."
                    };
                }

                if (statusCode == HttpStatusCode.OK && user != null)
                {
                    return new RegisterResult
                    {
                        Success = true,
                        Message = "User registered successfully."
                    };
                }

                return new RegisterResult
                {
                    Success = false,
                    Message = "An unexpected error occurred."
                };
            }
            catch (HttpRequestException)
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = "An error occurred while registering. Please try again later."
                };
            }
        }
    }
}
