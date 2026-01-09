using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Dtos.AdminDto
{
    public class SalesByDateDto
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OrdersCount { get; set; }
    }
}
