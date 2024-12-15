using ArtigosCientificosMvc.Models.User;

namespace ArtigosCientificosMvc.Components.Pages
{
    partial class Home
    {
        List<User> users = new List<User>();

        protected async override void OnInitialized()
        {
            base.OnInitialized();
            this.users = await this.homeService.GetUsers();
            
        }

    }
}
