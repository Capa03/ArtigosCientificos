using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArtigosCientificosMvc.Models.Review
{
    public class ReviewDescription
    {

        public int Id { get; set; }

        public int ReviewId { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
