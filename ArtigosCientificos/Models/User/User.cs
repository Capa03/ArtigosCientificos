namespace ArtigosCientificos.Api.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? Role { get; set; }
    }
}
