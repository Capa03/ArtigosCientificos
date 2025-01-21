namespace ArtigosCientificos.Api.Models.Article
{
    public class ArticleFilteredDTO
    {
        public string Username { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        public DateOnly InitalDate { get; set; }

        public DateOnly FinalDate { get; set; }

        public int Views {  get; set; }

        public int Downloads { get; set; }
    }
}
