using ArtigosCientificosMvc.Models.Login;

namespace ArtigosCientificosMvc.Components.Pages
{
    partial class Register
    {
        UserDTO userDTO = new UserDTO();
        string? ErrorMessage { get; set; }
        private string passwordInputType = "password";

        async Task OnRegister()
        {
            ErrorMessage = null;
            var result = await this.registerService.Register(userDTO);

            if (result.Success)
            {
                Navigation.NavigateTo("Login");
            }
            else
            {
                ErrorMessage = result.Message;
            }

            StateHasChanged();
        }


        private void TogglePasswordVisibility()
        {

            passwordInputType = passwordInputType == "password" ? "text" : "password";
        }

    }
}
