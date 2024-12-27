using ArtigosCientificos.Api.Models.Review;
using ArtigosCientificos.Api.Services.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtigosCientificos.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // Route: GET api/Review
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviews();
            if (reviews is NotFoundObjectResult notFound)
            {
                return NotFound(notFound.Value);
            }
            return Ok(reviews.Value);
        }

        // Route: POST api/Review
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDTO review)
        {
            if (review == null)
            {
                return BadRequest("Review data is required.");
            }

            var result = await _reviewService.CreateReview(review);
            if (result is BadRequestObjectResult badRequest)
            {
                return BadRequest(badRequest.Value);
            }

            return Ok(result.Value);
        }

        // Route: GET api/Review/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewById(id);
            if (review is NotFoundObjectResult notFound)
            {
                return NotFound("Review not found.");
            }
            return Ok(review.Value);
        }

        // Route: GET api/Review/status/{status}
        [HttpGet("status/{status}")]
        [Authorize(Roles = "Reviewer")]
        public async Task<IActionResult> GetReviewByStatus(string status)
        {
            var result = await _reviewService.GetReviewByStatus(status);
            if (result is NotFoundObjectResult notFound)
            {
                return NotFound($"No reviews found with status '{status}'.");
            }
            return Ok(result.Value);
        }

        // Route: PUT api/Review/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Reviewer")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewPutDTO review)
        {
            if (review == null)
            {
                return BadRequest("Review update data is required.");
            }

            var result = await _reviewService.UpdateReview(id, review);
            if (result is NotFoundObjectResult notFound)
            {
                return NotFound(notFound.Value);
            }
            return Ok(result.Value);
        }
    }
}
