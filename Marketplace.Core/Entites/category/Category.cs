using Marketplace.Core.Common.Base;
using Marketplace.Core.Entites.product;

namespace Marketplace.Core.Entites.category
{
    public class Category  : BaseAuditableEntity, ISoftDelete
    { 
        public string Name { get; set; }
        public string Slug { get; set; } = string.Empty;
        public int? ParentId { get; set; }

        public Category? Parent { get; set; }

        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }
    }
}
