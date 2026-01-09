using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Dtos.Products
{
    public record CreateImageProductDto
    (
        string Name,
        string Description,
        decimal Price,
        int StockQuantity,
        int CategoryId,
    IFormFile? Image = null);
    
}
