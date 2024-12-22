namespace ArtigosCientificosMvc.Models.User
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new();
    }
}