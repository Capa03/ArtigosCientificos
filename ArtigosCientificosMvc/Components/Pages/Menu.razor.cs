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
                await CheckIfLoggedIn();
                StateHasChanged();
            }
        }

        private async Task CheckIfLoggedIn()
        {
            try
            {
                if (TokenManager == null)
                {
                    throw new InvalidOperationException("TokenManager is not initialized.");
                }

                loggedIn = await TokenManager.IsUserAuthenticated();
                if (loggedIn)
                {
                    User? user = await TokenManager.GetUserAsync();

                    if (user != null && user.Role != null)
                    {
                        if (user.Role.Any(r => r.Name.Equals("Researcher", StringComparison.OrdinalIgnoreCase)) &&
                            user.Role.Any(r => r.Name.Equals("Reviewer", StringComparison.OrdinalIgnoreCase)))
                        {
                            userRole = "ResearcherReviewer";
                        }
                        else if (user.Role.Any(r => r.Name.Equals("Researcher", StringComparison.OrdinalIgnoreCase)))
                        {
                            userRole = "Researcher";
                        }
                        else if (user.Role.Any(r => r.Name.Equals("Reviewer", StringComparison.OrdinalIgnoreCase)))
                        {
                            userRole = "Reviewer";
                        }

                        username = user.Username ?? string.Empty;
                    }
                }
                else
                {
                    await TokenManager.RemoveTokenAsync();
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Interop call failed during pre-rendering: {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"Unexpected null reference: {ex.Message}");
            }
        }

        async Task Logout()
        {
            await TokenManager.RemoveTokenAsync();
            loggedIn = false;
            username = string.Empty;
            StateHasChanged();
            Navigation.NavigateTo("Home");
        }
    }
}
