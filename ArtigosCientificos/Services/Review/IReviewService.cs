using ArtigosCientificos.Api.Models.Review;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Services.Reviews
{
    /// <summary>
    /// Interface defining the contract for the review service.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Retrieves all reviews.
        /// </summary>
        /// <returns>An <see cref="ObjectResult"/> containing all reviews.</returns>
        Task<ObjectResult> GetAllReviews();

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="review">The review data transfer object (DTO) to be created.</param>
        /// <returns>An <see cref="ObjectResult"/> indicating the result of the review creation.</returns>
        Task<ObjectResult> CreateReview(ReviewDTO review);

        /// <summary>
        /// Retrieves a review by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>An <see cref="ObjectResult"/> containing the review with the specified ID.</returns>
        Task<ObjectResult> GetReviewById(int id);

        /// <summary>
        /// Updates an existing review by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the review to update.</param>
        /// <param name="review">The review data to update with.</param>
        /// <returns>An <see cref="ObjectResult"/> indicating the result of the review update.</returns>
        Task<ObjectResult> UpdateReview(int id, ReviewPutDTO review);

        /// <summary>
        /// Retrieves reviews by their status.
        /// </summary>
        /// <param name="Status">The status of the reviews to retrieve (e.g., "Pending", "Approved").</param>
        /// <returns>An <see cref="ObjectResult"/> containing reviews with the specified status.</returns>
        Task<ObjectResult> GetReviewByStatus(string Status);
    }
}
