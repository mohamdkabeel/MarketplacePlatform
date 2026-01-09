using Marketplace.Application.Dtos.Reviews;
using Marketplace.Application.IServices.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlatform.API.Controllers.Reviews
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateReviewDto dto)
        {
            string userId = User.FindFirst("id")?.Value ?? "anonymous";
            var review = await _reviewService.AddReviewAsync(userId, dto);
            return Ok(review);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var reviews = await _reviewService.GetReviewsByProductAsync(productId);
            return Ok(reviews);
        }
    }

}
