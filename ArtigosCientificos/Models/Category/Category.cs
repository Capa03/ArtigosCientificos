using ArtigosCientificos.Api.Models.Article;
using System.Text.Json.Serialization;

namespace ArtigosCientificos.Api.Models.Category
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Article.Article> Articles { get; set; } 
    }
}
