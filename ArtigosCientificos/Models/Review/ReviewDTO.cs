using System.Text.Json.Serialization;

namespace ArtigosCientificos.Api.Models.Review
{
    public class ReviewDTO
    {
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public string Status { get; set; } = "PENDING";
        public string Description { get; set; }
    }
}
