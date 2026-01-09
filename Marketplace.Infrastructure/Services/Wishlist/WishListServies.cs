
using Marketplace.Application.Dtos.Wishlist;
using Marketplace.Application.IServices.WishList;
using Marketplace.Core.Entites.wishlistItem;
using Marketplace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Services.Wishlist
{
  
 

    public class WishlistService : IWishlistService
    {
        private readonly ApplicationDbContext _context;

        public WishlistService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WishlistItem> AddToWishlistAsync(string userId, CreateWishlistItemDto dto)
        {
            var exists = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == dto.ProductId);

            if (exists != null) return exists;

            var wishlistItem = new WishlistItem
            {
                UserId = userId,
                ProductId = dto.ProductId
            };

            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();
            return wishlistItem;
        }

        public async Task<bool> RemoveFromWishlistAsync(string userId, int wishlistItemId)
        {
            var item = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.Id == wishlistItemId && w.UserId == userId);

            if (item == null) return false;

            _context.WishlistItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WishlistItem>> GetWishlistByUserAsync(string userId)
        {
            return await _context.WishlistItems
                .Include(w => w.Product)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }
    }

}
