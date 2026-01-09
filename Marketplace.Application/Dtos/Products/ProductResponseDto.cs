using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Dtos.Products
{
    public record ProductResponseDto(
        int Id, string Name, string Description, decimal Price, int StockQuantity, string ImageUrl, int CategoryId , string SellerId);
}
