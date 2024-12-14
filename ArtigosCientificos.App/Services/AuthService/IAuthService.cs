namespace ArtigosCientificos.App.Services.AuthService
{
    public interface IAuthService
    {
        bool IsUserAuthenticated();
        string? GetAuthToken();
        void clearToken();

        string? GetUsername();
    }
}
