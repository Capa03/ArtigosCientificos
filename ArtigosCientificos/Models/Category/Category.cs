using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ArtigosCientificos.Api.Models.Article;

namespace ArtigosCientificos.Api.Models.Category
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Article.Article>? Articles { get; set; }
    }
}
