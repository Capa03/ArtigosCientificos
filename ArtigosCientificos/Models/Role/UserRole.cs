

using ArtigosCientificos.Api.Models.User;

namespace ArtigosCientificos.Api.Models.Role
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<User.User> Users { get; set; } = new List<User.User>();
    }
}
