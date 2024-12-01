using ArtigosCientificos.Api.Models.Role;
using ArtigosCientificos.Api.Models.Token;

namespace ArtigosCientificos.Api.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Foreign key for Role
        public int RoleId { get; set; }
        public UserRole Role { get; set; } = new UserRole();

        // Navigation property for Tokens
        //public ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();
    }
}
