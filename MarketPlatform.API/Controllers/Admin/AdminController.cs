using Marketplace.Application.IServices.AdminReportService;
using Marketplace.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlatform.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get All Users
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new { u.Id, u.UserName, u.Email }).ToListAsync();
            return Ok(users);
        }

        // Get All Orders
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Select(o => new
                {
                    o.Id,
                    o.UserId,
                    o.Status,
                    o.TotalAmount,
                    o.CreatedAt,
                    Items = o.OrderItems.Select(oi => new
                    {
                        oi.ProductId,
                        oi.Product.Name,
                        oi.Quantity,
                        oi.Price
                    })
                }).ToListAsync();
            return Ok(orders);
        }

        // Get All Products
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.products
                .Include(p => p.Reviews)
                .Include(p => p.Category)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    Category = p.Category.Name,
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                    ReviewsCount = p.Reviews.Count
                }).ToListAsync();
            return Ok(products);
        }

        // Get All Reviews
        [HttpGet("reviews")]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _context.Reviews
                .Include(r => r.Product)
                .Select(r => new
                {
                    r.Id,
                    r.UserId,
                    r.ProductId,
                    r.Product.Name,
                    r.Rating,
                    r.Comment
                }).ToListAsync();
            return Ok(reviews);
        }

        [HttpGet("reports/summary")]
        public async Task<IActionResult> GetSummary([FromServices] IAdminReportService reportService)
        {
            var summary = new
            {
                TotalUsers = await reportService.TotalUsersAsync(),
                TotalOrders = await reportService.TotalOrdersAsync(),
                TotalRevenue = await reportService.TotalRevenueAsync()
            };

            return Ok(summary);
        }

        [HttpGet("reports/top-products")]
        public async Task<IActionResult> GetTopProducts([FromServices] IAdminReportService reportService, [FromQuery] int top = 5)
        {
            var data = await reportService.TopSellingProductsAsync(top);
            return Ok(data);
        }

        [HttpGet("reports/sales")]
        public async Task<IActionResult> GetSalesOverTime([FromServices] IAdminReportService reportService, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var data = await reportService.SalesOverTimeAsync(from, to);
            return Ok(data);
        }

    }

}
