using System.ComponentModel.DataAnnotations;

namespace ArtigosCientificos.Api.Models.User
{
    public class UserDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
