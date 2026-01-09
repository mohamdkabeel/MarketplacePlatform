using Marketplace.Application.Dtos.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlatform.API.Controllers.Cart
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var cart = await _cartService.GetCartByUserAsync(userId);
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(CreateCartItemDto dto)
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var item = await _cartService.AddToCartAsync(userId, dto);
            return Ok(item);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCart(UpdateCartItemDto dto)
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var item = await _cartService.UpdateCartItemAsync(userId, dto);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCart(int id)
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var success = await _cartService.RemoveFromCartAsync(userId, id);
            return success ? Ok("Removed") : NotFound();
        }
    }

}
