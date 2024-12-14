using System.Net;
using System.Text.Json;
using ArtigosCientificos.App.Models.Register;
using ArtigosCientificos.App.Models.User;
using ArtigosCientificos.App.Services.ApiService;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.App.Services.RegisterService
{
    public class RegisterService : IRegisterService
    {
        private readonly ApiService.ApiService _apiService;
        private readonly ConfigServer _configServer;
        public RegisterService(ApiService.ApiService apiService, ConfigServer configServer)
        {
            this._apiService = apiService;
            _configServer = configServer;
        }
        public async Task<string> Register(RegisterDTO registerDTO)
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
