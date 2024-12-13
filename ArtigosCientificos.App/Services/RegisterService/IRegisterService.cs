using ArtigosCientificos.App.Models.Register;
using ArtigosCientificos.App.Models.User;

namespace ArtigosCientificos.App.Services.RegisterService
{
    public interface IRegisterService
    {
       Task<string> Register(RegisterDTO registerDTO);
    }
}
