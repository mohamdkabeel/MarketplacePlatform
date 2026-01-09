using Marketplace.Application.Dtos.Reviews;
using Marketplace.Application.IServices.Notification;
using Marketplace.Application.IServices.Reviews;
using Marketplace.Core.Entites.review;
using Marketplace.Infrastructure.Data;
using Marketplace.Infrastructure.Services.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Infrastructure.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public ReviewService(ApplicationDbContext context ,INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task<Review> AddReviewAsync(string userId, CreateReviewDto dto)
        {
            var product = await _context.products.FindAsync(dto.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var review = new Review
            {
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                UserId = userId
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            
            if (product != null)
            {
                await _notificationService.SendNotificationAsync(product.CreatedBy, $"Your product {product.Name} got a new review!");
            }
            return review;
        }

        public async Task<IEnumerable<Review>> GetReviewsByProductAsync(int productId)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId).ToListAsync();
        }
    }

}
