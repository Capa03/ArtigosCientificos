using ArtigosCientificosMvc.Models.Login;

namespace ArtigosCientificosMvc.Service.Login
{
    public interface ILoginService
    {
        Task<LoginRequest> Login(UserDTO userDTO);
    }
}
