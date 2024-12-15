using ArtigosCientificosMvc.Models.Login;

namespace ArtigosCientificosMvc.Service.Register
{
    public interface IRegisterService
    {
        Task<string> Register(UserDTO userDTO);
    }
}
