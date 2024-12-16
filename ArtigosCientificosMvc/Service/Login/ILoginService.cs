using ArtigosCientificosMvc.Models.Login;

namespace ArtigosCientificosMvc.Service.Login
{
    public interface ILoginService
    {
        Task<LoginResult> Login(UserDTO userDTO);
    }
}
