using Marketplace.Application.Dtos.Wishlist;
using Marketplace.Application.IServices.WishList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlatform.API.Controllers.WishList
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var list = await _wishlistService.GetWishlistByUserAsync(userId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist(CreateWishlistItemDto dto)
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var item = await _wishlistService.AddToWishlistAsync(userId, dto);
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWishlist(int id)
        {
            string userId = User.FindFirst("sub")?.Value ?? throw new Exception("User not found");
            var success = await _wishlistService.RemoveFromWishlistAsync(userId, id);
            return success ? Ok("Removed") : NotFound();
        }
    }

}
