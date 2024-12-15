using ArtigosCientificosMvc.Models.User;

namespace ArtigosCientificosMvc.Components.Pages
{
    partial class Home
    {
        private List<User> users;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                users = await homeService.getUsers();
                StateHasChanged();
            }
        }
    }
}
