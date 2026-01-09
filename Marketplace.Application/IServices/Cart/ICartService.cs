using Marketplace.Application.Dtos.Cart;
using Marketplace.Core.Entites.cartItem;
using Marketplace.Core.Entites;

public interface ICartService
{
    Task<CartItem> AddToCartAsync(string userId, CreateCartItemDto dto);
    Task<CartItem> UpdateCartItemAsync(string userId, UpdateCartItemDto dto);
    Task<bool> RemoveFromCartAsync(string userId, int cartItemId);
    Task<IEnumerable<CartItem>> GetCartByUserAsync(string userId);
}

