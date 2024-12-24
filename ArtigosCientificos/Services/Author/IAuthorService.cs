using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.Author
{
    public interface IAuthorService
    {
        Task<ObjectResult> GetAllUsers();
        Task<ObjectResult> GetUserById(int id);
    }
}
