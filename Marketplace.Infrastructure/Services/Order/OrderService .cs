using Marketplace.Application.Dtos.Orders;
using Marketplace.Application.IServices.Email;
using Marketplace.Application.IServices.Notification;
using Marketplace.Application.IServices.Order;
using Marketplace.Core.Entites.order;
using Marketplace.Infrastructure.Data;
using Marketplace.Infrastructure.Services.Email;
using Marketplace.Infrastructure.Services.Notification;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Services.Order
{
    


    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;

        public OrderService(ApplicationDbContext context ,IEmailService emailService , INotificationService notificationService )
        {
            _context = context;
            _emailService = emailService;
            _notificationService = notificationService;
        }

        public async Task<OrderDto> CreateOrderAsync(string userId, CreateOrderDto dto)
        {
            var order = new Core.Entites.order.Order { UserId = userId };

            decimal total = 0;

            foreach (var itemDto in dto.Items)
            {
                var product = await _context.products.FindAsync(itemDto.ProductId);
                if (product == null) throw new Exception($"Product {itemDto.ProductId} not found");

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    Price = product.Price
                };

                total += product.Price * itemDto.Quantity;
                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;

            _context.Orders.Add(order);

            // Optionally: clear user's cart
            var cartItems = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(order.UserId);
            var userEmail = user?.Email ?? "";

            // Email
            await _emailService.SendEmailAsync(userEmail, "Order Placed", $"Your order #{order.Id} has been placed successfully!");

            // SignalR
            await _notificationService.SendNotificationAsync(userId, $"Your order #{order.Id} has been placed!");


            return await GetOrderByIdAsync(order.Id, userId) ?? throw new Exception("Order creation failed");


        }


        public async Task<IEnumerable<OrderDto>> GetOrdersByUserAsync(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            });
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            order.Status = status;
            await _context.SaveChangesAsync();
            var user = await _context.Users.FindAsync(order.UserId);
            var userEmail = user?.Email ?? "";

            await _notificationService.SendNotificationAsync(order.UserId, $"Your order #{order.Id} status has changed to {status}");
            await _emailService.SendEmailAsync(userEmail, "Order Status Update", $"Your order #{order.Id} status is now {status}");
            return true;
        }
    }

}
