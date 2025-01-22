using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ArtigosCientificos.Api.Models.User;

namespace ArtigosCientificos.Api.Models.Article
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Keywords { get; set; }
        public string File { get; set; }
        [DataType(DataType.Date)]
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [DataType(DataType.Date)]
        public DateOnly? ReviewedAt { get; set; } 
        public int UserId { get; set; }
        [JsonIgnore]
        public User.User User { get; set; }
        [JsonIgnore]
        public List<Review.Review> Reviews { get; set; }
        public int CategoryId { get; set; }
        public Category.Category? Category { get; set; }

        public int? Views { get; set; } = 0;
        public int? Downloads { get; set; } = 0;
    }
}
