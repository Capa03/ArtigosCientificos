namespace ArtigosCientificos.Api.Models.Review
{
    public class ReviewWithArticleDTO
    {
        public int ReviewId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }

        // Article related properties
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Keywords { get; set; }
        public string File { get; set; }
    }
}
