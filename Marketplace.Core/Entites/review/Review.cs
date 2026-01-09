using Marketplace.Core.Entites.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Core.Entites.review
{
    public class Review
    {
        public int Id { get; set; }

        public int Rating { get; set; }   // 1-5
        public string Comment { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }

}
