

using ArtigosCientificosMvc.Models.Review;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificosMvc.Service.Review
{
    public interface IReviewService
    {
        Task<List<ReviewWithArticleDTO>> GetAllReviewsByStatus(string Status);
        Task<ReviewWithArticleDTO> GetReviewById(int id);
        Task<ObjectResult> UpdateReview(int id, ReviewPutDTO review);
    }
}
