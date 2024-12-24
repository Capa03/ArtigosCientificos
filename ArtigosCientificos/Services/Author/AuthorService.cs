using ArtigosCientificos.Api.Data;
using ArtigosCientificos.Api.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtigosCientificos.Api.Services.Author
{
    public class AuthorService : IAuthorService
    {
        private readonly DataContext _context;
        public AuthorService(DataContext dataContext)
        {
            _context = dataContext;
        }

        /// <summary>
        /// Retrieves all users from the database along with their roles and tokens.
        /// </summary>
        /// <returns>A list of users with their associated roles and tokens.</returns>

        public async Task<ObjectResult> GetAllUsers()
        {
            List<User> users = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Token).ToListAsync();

            if (users == null)
            {
                return new NotFoundObjectResult("No users found.");
            }

            return new OkObjectResult(users);
        }

        public async Task<ObjectResult> GetUserById(int id)
        {
            User user = _context.Users
                .Include(u => u.Role)
                .Include(u => u.Token)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found.");
            }

            return new OkObjectResult(user);
        }
    }
}
