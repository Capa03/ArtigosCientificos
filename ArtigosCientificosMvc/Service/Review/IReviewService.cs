using ArtigosCientificosMvc.Models.Review;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificosMvc.Service.Review
{
    /// <summary>
    /// Defines the methods related to the review functionality, including fetching and updating reviews.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        /// Asynchronously retrieves a list of reviews filtered by their status.
        /// </summary>
        /// <param name="Status">The status of the reviews to filter by (e.g., "Pending", "Approved", "Rejected").</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result is a list of <see cref="ReviewWithArticleDTO"/> objects containing the reviews and their associated articles.</returns>
        Task<List<ReviewWithArticleDTO>> GetAllReviewsByStatus(string Status);

        /// <summary>
        /// Asynchronously retrieves a specific review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result is a <see cref="ReviewWithArticleDTO"/> object containing the review and the associated article.</returns>
        Task<ReviewWithArticleDTO> GetReviewById(int id);

        /// <summary>
        /// Asynchronously updates a review based on the provided review details.
        /// </summary>
        /// <param name="id">The unique identifier of the review to update.</param>
        /// <param name="review">An object containing the new data for the review (e.g., updated status, comments).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. The task result is an <see cref="ObjectResult"/> indicating the result of the update operation.</returns>
        Task<ObjectResult> UpdateReview(int id, ReviewPutDTO review);
    }
}
