using ArtigosCientificosMvc.Models.Login;
using ArtigosCientificosMvc.Service.Token;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificosMvc.Components.Pages
{
    partial class Login
    {
        UserDTO userDTO = new UserDTO();
        string? ErrorMessage { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await this.TokenManager.RemoveTokenAsync();
                StateHasChanged();
            }
        }

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
