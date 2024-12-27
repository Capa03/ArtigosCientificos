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
        public string CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now).ToString();
        [DataType(DataType.Date)]
        public string UpdatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now).ToString();
        public int UserId { get; set; }
        [JsonIgnore]
        public User.User User { get; set; }
        
        [JsonIgnore]
        public List<Review.Review> Reviews { get; set; }
    }
}
