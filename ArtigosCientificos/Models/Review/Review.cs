using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ArtigosCientificos.Api.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ArtigosCientificos.Api.Models.Review
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User.User User { get; set; }
        public int ArticleId { get; set; }
        [JsonIgnore]
        public Article.Article Article { get; set; }
        public string Status { get; set; } = "PENDING";
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
