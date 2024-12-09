using ArtigosCientificos.App.Models.Login;
using ArtigosCientificos.App.Models.User;

namespace ArtigosCientificos.App.Services.LoginService
{
    public interface ILoginService
    {
        public Task<LoginRequest> Login(UserDTO userDTO);
    }
}
