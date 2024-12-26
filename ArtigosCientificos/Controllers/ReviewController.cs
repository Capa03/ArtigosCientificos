using System.Net;
using ArtigosCientificos.Api.Models.Review;
using ArtigosCientificos.Api.Services.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
            ObjectResult reviews = await reviewService.GetAllReviews();
            if (reviews.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(reviews.Value);
            }
            return Ok(reviews.Value);
        }

        // Route: POST api/Review
        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewDTO review)
        {
            ObjectResult reviews = await reviewService.CreateReview(review);
            if (reviews.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                return BadRequest(reviews.Value);
            }
            return Ok(reviews.Value);
        }

        // Route: GET api/Review/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            ObjectResult review = await reviewService.GetReviewById(id);
            if (review.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound("Review not found");
            }
            return Ok(review.Value);
        }

        // Route: GET api/Review/status
        [HttpGet("status/{Status}")]
        [Authorize(Roles = "Reviewer")]
        public async Task<IActionResult> GetReviewByStatus(string Status)
        {
            ObjectResult review = await reviewService.GetReviewByStatus(Status);
            if (review.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound("No pending reviews found");
            }
            return Ok(review.Value);
        }

        // Route: PUT api/Review/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Reviewer")]
        public async Task<IActionResult> UpdateReview(int id, ReviewPutDTO review)
        {
            ObjectResult reviews = await reviewService.UpdateReview(id, review);
            if (reviews.StatusCode == (int)HttpStatusCode.NotFound)
            {
                return NotFound(reviews.Value);
            }
            return Ok(reviews.Value);
        }
    }
}
