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
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public List<UserRole>? Role { get; set; } = new();

        public List<UserToken> Token { get; set; } = new();

        public List<Article.Article> Articles { get; set; } = new();
    }
}
