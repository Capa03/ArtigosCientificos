namespace ArtigosCientificos.App.Models.Token
{
    public class UserToken
    {
        public int Id { get; set; }
        public string TokenValue { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expired { get; set; }

        public int UserId { get; set; }
        public User.User User { get; set; }
    }
}
