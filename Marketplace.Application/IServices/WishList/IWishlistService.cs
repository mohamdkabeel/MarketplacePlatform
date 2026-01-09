using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.IServices.WishList
{
    using Marketplace.Application.Dtos.Wishlist;
    using Marketplace.Core.Entites.wishlistItem;
 

    public interface IWishlistService
    {
        Task<WishlistItem> AddToWishlistAsync(string userId, CreateWishlistItemDto dto);
        Task<bool> RemoveFromWishlistAsync(string userId, int wishlistItemId);
        Task<IEnumerable<WishlistItem>> GetWishlistByUserAsync(string userId);
    }

}
