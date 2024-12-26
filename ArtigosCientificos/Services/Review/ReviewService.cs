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
            List<Review> reviews = _context.Reviews.ToList();

            if (reviews.Count == 0)
            {
                return new NotFoundObjectResult("No reviews found");
            }

            return new OkObjectResult(reviews);
        }

        public async Task<ObjectResult> GetPendingReview()
        {
            List<Review> reviews = _context.Reviews.Where(r => r.Status == "PENDING").ToList();

            if (reviews.Count == 0)
            {
                return new NotFoundObjectResult("No pending reviews found");
            }

            return new OkObjectResult(reviews);
        }

        public async Task<ObjectResult> GetReviewById(int id)
        {
            Review review = _context.Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                return new NotFoundObjectResult("Review not found");
            }
            return new OkObjectResult(review);

        }

        public async Task<ObjectResult> UpdateReview(int id, ReviewDTO review)
        {
            Review reviewToUpdate = _context.Reviews.FirstOrDefault(r => r.Id == id);
            if (reviewToUpdate == null)
            {
                return new NotFoundObjectResult("Review not found");
            }

            _context.Entry(reviewToUpdate).CurrentValues.SetValues(review);
            await _context.SaveChangesAsync();
            return new OkObjectResult(reviewToUpdate);
        }
    }
}
