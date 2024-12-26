using System.ComponentModel.DataAnnotations;

namespace ArtigosCientificos.Api.Models.Article
{
    public class ArticleDTO
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Keywords { get; set; }
        public string File { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
    }
}
