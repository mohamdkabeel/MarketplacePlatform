using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Core.Common;
using Marketplace.Core.Common.Base; 
using Marketplace.Core.Entites.category;
using Marketplace.Core.Entites.review;

namespace Marketplace.Core.Entites.product
{
    public class Product : BaseAuditableEntity
    {
        public int Id { get; set; }


        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty ;


        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public string OwnerId { get; set; } = string.Empty;  // UserId للبائع


        //public ApplicationUser Seller { get; set; } = null;

        //public bool IsDeleted { get; set; } = false;
    }
}
