using System.ComponentModel.DataAnnotations;

namespace ArtigosCientificos.App.Models.Role
{
    public class UserRole
    {
 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<User.User> Users { get; set; } = new();
    }
}
