using ArtigosCientificos.Api.Data;
using ArtigosCientificos.Api.Models.Article;
using ArtigosCientificos.Api.Models.Review;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtigosCientificos.Api.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _context;

        public ReviewService(DataContext context)
        {
            _context = context;
        }

        public async Task<ObjectResult> CreateReview(ReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
                return new BadRequestObjectResult("Invalid review data.");

            var review = new Review
            {
                ArticleId = reviewDTO.ArticleId,
                UserId = reviewDTO.UserId,
                Status = reviewDTO.Status,
                Description = reviewDTO.Description
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return new OkObjectResult(review);
        }

        public async Task<ObjectResult> GetAllReviews()
        {
            var reviews = await _context.Reviews.ToListAsync();

            if (reviews.Count == 0)
                return new NotFoundObjectResult("No reviews found.");

            return new OkObjectResult(reviews);
        }

        public async Task<ObjectResult> GetReviewByStatus(string status)
        {
            var reviewsWithArticles = await _context.Reviews
                .Where(r => r.Status == status)
                .Join(_context.Articles,
                    review => review.ArticleId,
                    article => article.Id,
                    (review, article) => new ReviewWithArticleDTO
                    {
                        AuthorId = review.UserId,
                        ReviewId = review.Id,
                        Status = review.Status,
                        ArticleId = article.Id,
                        Title = article.Title,
                        Abstract = article.Abstract,
                        Keywords = article.Keywords,
                        Description = article.Reviews.FirstOrDefault(r => r.Id == review.Id).Description,
                        File = article.File
                    })
                .ToListAsync();

            if (!reviewsWithArticles.Any())
                return new NotFoundObjectResult($"No reviews with status '{status}' found.");

            return new OkObjectResult(reviewsWithArticles);
        }

        public async Task<ObjectResult> GetReviewById(int id)
        {
            var reviewWithArticle = await _context.Reviews
                .Where(r => r.Id == id)
                .Join(_context.Articles,
                    review => review.ArticleId,
                    article => article.Id,
                    (review, article) => new ReviewWithArticleDTO
                    {
                        AuthorId = review.UserId,
                        ReviewId = review.Id,
                        Status = review.Status,
                        ArticleId = article.Id,
                        Title = article.Title,
                        Abstract = article.Abstract,
                        Keywords = article.Keywords,
                        Description = article.Reviews.FirstOrDefault(r => r.Id == review.Id).Description,
                        File = article.File
                    })
                .FirstOrDefaultAsync();

            if (reviewWithArticle == null)
                return new NotFoundObjectResult($"No review with ID '{id}' found.");

            return new OkObjectResult(reviewWithArticle);
        }

        public async Task<ObjectResult> UpdateReview(int id, ReviewPutDTO reviewDTO)
        {
            var reviewToUpdate = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);

            if (reviewToUpdate == null)
                return new NotFoundObjectResult("Review not found.");

            if (!string.IsNullOrEmpty(reviewDTO.Description))
            {
                var reviewDescription = new ReviewDescription
                {
                    ReviewId = id,
                    Description = reviewDTO.Description
                };
                await _context.ReviewDescriptions.AddAsync(reviewDescription);
            }

            _context.Entry(reviewToUpdate).CurrentValues.SetValues(reviewDTO);
            await _context.SaveChangesAsync();
            return new OkObjectResult(reviewToUpdate);
        }
    }
}
