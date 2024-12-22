namespace ArtigosCientificosMvc.Components.Pages
{
    partial class Menu
    {

        private bool loggedIn = false;
        private string username = string.Empty;
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
            loggedIn = await TokenManager.IsUserAuthenticated();
            if (loggedIn)
            {
                username = await TokenManager.GetUsername();
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
