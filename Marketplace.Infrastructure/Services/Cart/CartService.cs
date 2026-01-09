using Marketplace.Application.Dtos.Cart;
using Marketplace.Core.Entites.cartItem;
using Marketplace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Services.Cart
{
    
 

    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> AddToCartAsync(string userId, CreateCartItemDto dto)
        {
            var existing = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == dto.ProductId);

            if (existing != null)
            {
                existing.Quantity += dto.Quantity;
                await _context.SaveChangesAsync();
                return existing;
            }

            var cartItem = new CartItem
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(string userId, UpdateCartItemDto dto)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == dto.Id && c.UserId == userId);

            if (item == null) throw new Exception("Cart item not found");

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> RemoveFromCartAsync(string userId, int cartItemId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (item == null) return false;

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CartItem>> GetCartByUserAsync(string userId)
        {
            return await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }

}
