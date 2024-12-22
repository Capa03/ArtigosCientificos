
namespace ArtigosCientificos.App.Views.Components.Menu
{
    public partial class Menu
    {
        private bool loggedIn = false;
        private string username = string.Empty;


        protected override void OnInitialized()
        {
            base.OnInitialized();
            CheckIfLoggedIn();
        }

        private void CheckIfLoggedIn()
        {
            loggedIn = AuthService.IsUserAuthenticated();
            if (loggedIn)
            {
                username = AuthService.GetUsername();
            }
        }
    }
}
