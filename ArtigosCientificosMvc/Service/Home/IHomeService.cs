using ArtigosCientificosMvc.Models.User;

namespace ArtigosCientificosMvc.Service.Home
{
    public interface IHomeService
    {
        Task<List<User>> GetUsers();
    }
}
