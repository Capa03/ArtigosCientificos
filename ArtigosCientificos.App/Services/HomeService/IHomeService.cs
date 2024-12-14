using ArtigosCientificos.App.Models.User;

namespace ArtigosCientificos.App.Services.HomeService
{
    public interface IHomeService
    {
        Task<List<User>> GetUsers();
    }
}
