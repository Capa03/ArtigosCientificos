using ArtigosCientificosMvc.Models.Login;

namespace ArtigosCientificosMvc.Components.Pages
{
    partial class Register
    {
        UserDTO userDTO = new UserDTO();

        async Task OnRegister()
        {
            string response = await this.registerService.Register(userDTO);


            if (response == "User registered successfully.")
            {
                Navigation.NavigateTo("login");
            }
        }
    }
}
