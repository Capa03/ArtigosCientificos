namespace ArtigosCientificosMvc.Models.Article
{
    public class Article
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Keywords { get; set; }
        public string File { get; set; }
        public string Status { get; set; } = "PENDING";
        public int UserId { get; set; }
    }
}
