using Marketplace.Application.Dtos.AdminDto;
using Marketplace.Application.IServices.AdminReportService;
using Marketplace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Services.AdminServices
{

    public class AdminReportService : IAdminReportService
    {
        private readonly ApplicationDbContext _context;

        public AdminReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> TotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> TotalOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> TotalRevenueAsync()
        {
            return await _context.Orders.SumAsync(o => o.TotalAmount);
        }

        public async Task<List<TopProductDto>> TopSellingProductsAsync(int top = 5)
        {
            var data = await _context.OrderItems
                .Include(oi => oi.Product)
                .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
                .Select(g => new TopProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    QuantitySold = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.Price)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(top)
                .ToListAsync();

            return data;
        }

        public async Task<List<SalesByDateDto>> SalesOverTimeAsync(DateTime from, DateTime to)
        {
            var data = await _context.Orders
                .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new SalesByDateDto
                {
                    Date = g.Key,
                    OrdersCount = g.Count(),
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            return data;
        }
    }

}
