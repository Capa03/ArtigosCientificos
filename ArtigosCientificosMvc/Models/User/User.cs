

namespace ArtigosCientificosMvc.Models.User
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public List<UserRole>? Role { get; set; }

        public List<UserToken>? Token { get; set; }
    }
}
