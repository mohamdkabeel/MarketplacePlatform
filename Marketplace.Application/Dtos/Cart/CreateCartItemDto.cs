using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Dtos.Cart
{
    public class CreateCartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

}
