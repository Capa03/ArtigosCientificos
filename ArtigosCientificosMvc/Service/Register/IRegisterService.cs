using ArtigosCientificosMvc.Models.Login;
using ArtigosCientificosMvc.Models.Register;

namespace ArtigosCientificosMvc.Service.Register
{
    public interface IRegisterService
    {
        Task<RegisterResult> Register(UserDTO registerDTO);
    }
}
