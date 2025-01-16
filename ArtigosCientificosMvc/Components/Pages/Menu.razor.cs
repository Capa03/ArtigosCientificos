using ArtigosCientificosMvc.Models.User;

namespace ArtigosCientificosMvc.Components.Pages
{
    partial class Menu
    {

        private bool loggedIn = false;
        private string username = string.Empty;
        private string? userRole;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                await this.CheckIfLoggedIn();
                StateHasChanged();
            }
        }

        private async Task CheckIfLoggedIn()
        {
            try
            {
                loggedIn = await TokenManager.IsUserAuthenticated();
                if (loggedIn)
                {
                    
                    userRole = await TokenManager.GetUserRoleAsync();
                    User user = await TokenManager.GetUserAsync();
                    if (user.Username == null)
                    {
                        await TokenManager.RemoveTokenAsync();
                    }
                    username = user.Username;
                }
                else
                {
                    await TokenManager.RemoveTokenAsync();
                }
            }
            catch (InvalidOperationException ex)
            {
                // Log or handle the pre-rendering error
                Console.WriteLine($"Interop call failed during pre-rendering: {ex.Message}");
            }
        }



        async Task Logout()
        {
            await TokenManager.RemoveTokenAsync();
            loggedIn = false;
            username = string.Empty;
            StateHasChanged();
            Navigation.NavigateTo("Login");
        }
    }
}
