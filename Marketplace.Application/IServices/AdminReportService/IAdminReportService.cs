using Marketplace.Application.Dtos.AdminDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.IServices.AdminReportService
{
    public interface IAdminReportService
    {
        Task<int> TotalUsersAsync();
        Task<int> TotalOrdersAsync();
        Task<decimal> TotalRevenueAsync();
        Task<List<TopProductDto>> TopSellingProductsAsync(int top = 5);
        Task<List<SalesByDateDto>> SalesOverTimeAsync(DateTime from, DateTime to);
    }

}
