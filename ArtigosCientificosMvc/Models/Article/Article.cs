using System.ComponentModel.DataAnnotations;

namespace ArtigosCientificosMvc.Models.Article
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title must be less than 100 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Abstract is required.")]
        [StringLength(500, ErrorMessage = "Abstract must be less than 500 characters.")]
        public string Abstract { get; set; }

        [Required(ErrorMessage = "Keywords are required.")]
        [StringLength(200, ErrorMessage = "Keywords must be less than 200 characters.")]
        public string Keywords { get; set; }

        public string File { get; set; }

        public string Status { get; set; } = "PENDING";

        public int UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
