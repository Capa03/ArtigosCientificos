using System.ComponentModel.DataAnnotations;
using ArtigosCientificos.App.Models.Role;
using ArtigosCientificos.App.Models.Token;

namespace ArtigosCientificos.App.Models.User
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public List<UserRole>? Role { get; set; }

        public List<UserToken>? Token { get; set; } 
    }
}
