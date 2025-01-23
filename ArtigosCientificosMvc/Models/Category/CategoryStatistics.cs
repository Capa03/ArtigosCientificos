namespace ArtigosCientificosMvc.Models.Category
{
    public class CategoryStatistics
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ArticleCount { get; set; }
        public int TotalViews { get; set; }
        public int TotalDownloads { get; set; }
    }
}
