using System.ComponentModel.DataAnnotations;
using ArtigosCientificos.Api.Models.Role;
using ArtigosCientificos.Api.Models.Token;

namespace ArtigosCientificos.Api.Models.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Navigation property for user roles (many-to-many)
        public int? RoleId { get; set; } 
        public List<UserRole>? Role { get; set; } = new();

        // Navigation property for tokens (one-to-many)
        public List<UserToken> Token { get; set; } = new();
    }
}
