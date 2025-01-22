namespace ArtigosCientificosMvc.Models.Article
{
    public class ArticleStatistics
    {
        public int TotalArticles { get; set; }
        public int TotalViews { get; set; }
        public int TotalDownloads { get; set; }
        public string MostViewedCategory { get; set; }
        public int MostViewedCategoryCount { get; set; }
    }
}
