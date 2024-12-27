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
            Review review = new Review
            {
                ArticleId = reviewDTO.ArticleId,
                UserId = reviewDTO.UserId,
                Status = reviewDTO.Status,
                Description = reviewDTO.Description
            };

            if (review == null)
            {
                return new BadRequestObjectResult("Error creating review");
            }

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return new OkObjectResult(review);
        }

        public async Task<ObjectResult> GetAllReviews()
        {
            List<Review> reviews = _context.Reviews.Include(r => r.Description).ToList();

            if (reviews.Count == 0)
            {
                return new NotFoundObjectResult("No reviews found");
            }

            return new OkObjectResult(reviews);
        }

        public async Task<ObjectResult> GetReviewByStatus(string status)
        {
            // Fetch reviews and their associated articles in a single query
            var reviewsWithArticles = await _context.Reviews
                .Where(r => r.Status == status)
                .AsSplitQuery() // This will split the query to load the Reviews and Articles separately
                .Join(
                    _context.Articles,
                    review => review.ArticleId,
                    article => article.Id,
                    (review, article) => new ReviewWithArticleDTO
                    {
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

            // Check if no reviews and articles were found
            if (!reviewsWithArticles.Any())
            {
                return new NotFoundObjectResult($"No reviews with status '{status}' found.");
            }

            return new OkObjectResult(reviewsWithArticles);
        }

        public async Task<ObjectResult> GetReviewById(int id)
        {
            // Fetch a single review with its associated article
            var reviewWithArticle = await _context.Reviews
                .Where(r => r.Id == id)
                .Join(
                    _context.Articles,
                    review => review.ArticleId,
                    article => article.Id,
                    (review, article) => new ReviewWithArticleDTO
                    {
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

            // Check if no review with the given ID was found
            if (reviewWithArticle == null)
            {
                return new NotFoundObjectResult($"No review with ID '{id}' found.");
            }

            return new OkObjectResult(reviewWithArticle);
        }


        public async Task<ObjectResult> UpdateReview(int id, ReviewPutDTO review)
        {
            Review reviewToUpdate = _context.Reviews.FirstOrDefault(r => r.Id == id);
            if (reviewToUpdate == null)
            {
                return new NotFoundObjectResult("Review not found");
            }

            ReviewDescription reviewDescription = new ReviewDescription
            {
                ReviewId = id,
                Description = review.Description
            };
            if (!string.IsNullOrEmpty(review.Description))
            {
                await _context.ReviewDescriptions.AddAsync(reviewDescription);
                await _context.SaveChangesAsync();
            }
            // Add a new review description

            _context.Entry(reviewToUpdate).CurrentValues.SetValues(review);
            await _context.SaveChangesAsync();
            return new OkObjectResult(reviewToUpdate);
        }

        public Task<ObjectResult> GetDescriptionsFromReview(int reviewId)
        {
            throw new NotImplementedException();
        }
    }
}
