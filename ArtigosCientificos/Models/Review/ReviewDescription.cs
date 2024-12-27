using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArtigosCientificos.Api.Models.Review
{
    public class ReviewDescription
    {
        [Key]
        public int Id { get; set; }

        public int ReviewId { get; set; }
        [JsonIgnore]
        public Review Review { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
