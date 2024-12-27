using ArtigosCientificos.Api.Models.Review;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.Reviews
{
    public interface IReviewService
    {
        Task<ObjectResult> GetAllReviews();
        Task<ObjectResult> CreateReview(ReviewDTO review);

        Task<ObjectResult> GetReviewById(int id);
        Task<ObjectResult> UpdateReview(int id, ReviewPutDTO review);
        Task<ObjectResult> GetReviewByStatus(string Status);
        Task<ObjectResult> GetDescriptionsFromReview(int reviewId);
    }
}
