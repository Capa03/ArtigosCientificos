using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArtigosCientificosMvc.Models.Review
{
    public class Review
    {

        public int Id { get; set; }
        public int UserId { get; set; }

        public int ArticleId { get; set; }

        public string Status { get; set; } = "PENDING";
        public List<ReviewDescription> Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
