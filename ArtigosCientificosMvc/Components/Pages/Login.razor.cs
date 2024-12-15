using ArtigosCientificosMvc.Models.Login;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificosMvc.Components.Pages
{
    partial class Login
    {
        UserDTO userDTO = new UserDTO();
        string? ErrorMessage { get; set; }

        async Task OnLogin()
        {
            ErrorMessage = null;

            LoginRequest? loginRequest = await this.loginService.Login(userDTO);
            if (loginRequest != null)
            {
                if (loginRequest.StatusCode != 200)
                {
                    this.ErrorMessage = "Invalid Credentials.";
                    return;
                }
                await LocalStorage.SetAsync("token", loginRequest.Value);
                Navigation.NavigateTo("Home");
            }
            else
            {
                this.ErrorMessage = "Login failed. Please try again.";
            }
        }
    }
}
