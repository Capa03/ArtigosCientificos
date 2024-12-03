

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ArtigosCientificos.Api.Models.User;

namespace ArtigosCientificos.Api.Models.Role
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public List<User.User> Users { get; set; } = new();
    }
}
