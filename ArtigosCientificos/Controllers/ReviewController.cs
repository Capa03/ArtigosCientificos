using System.Net;
using ArtigosCientificos.Api.Models.Review;
using ArtigosCientificos.Api.Services.Reviews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        // Route: GET api/Review
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await reviewService.GetAllReviews();
            if (reviews.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound("No reviews found");
            }
            return Ok(reviews);
        }

        // Route: POST api/Review
        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewDTO review)
        {
            var result = await reviewService.CreateReview(review);
            if (result.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                return BadRequest("Error creating review");
            }
            return Ok(result);
        }

        // Route: GET api/Review/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await reviewService.GetReviewById(id);
            if (review.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound("Review not found");
            }
            return Ok(review);
        }

        // Route: GET api/Review/pending
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingReview()
        {
            var review = await reviewService.GetPendingReview();
            if (review.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound("No pending reviews found");
            }
            return Ok(review);
        }

        // Route: PUT api/Review/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReview(int id, ReviewDTO review)
        {
            var result = await reviewService.UpdateReview(id, review);
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound("Review not found");
            }
            return Ok(result);
        }
    }
}
