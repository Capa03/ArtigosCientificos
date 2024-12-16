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

            LoginResult loginRequest = await this.loginService.Login(userDTO);
            if (loginRequest.Success)
            {
                Navigation.NavigateTo("Home", true);
            }
            else
            {
                this.ErrorMessage = loginRequest.Message;
            }
        }
    }
}
