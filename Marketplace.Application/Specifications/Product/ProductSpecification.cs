
namespace Marketplace.Application.Specifications.Product
{
    public class ProductSpecification : BaseSpecification<Core.Entites.product.Product>
    {
        public ProductSpecification(string? search , int? categoryId)
        {
            Criteria = p =>
       (string.IsNullOrEmpty(search) ||
           p.Name.Contains(search) ||
           p.Description.Contains(search))
       &&
       (!categoryId.HasValue || p.CategoryId == categoryId);
        }
    }
}
