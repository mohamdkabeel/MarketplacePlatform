using Marketplace.Application.Dtos.Products;
using Marketplace.Core.Entites;
using Marketplace.Core.Entites.category;
using Marketplace.Core.Entites.product;
using System.Threading.Tasks;
namespace Marketplace.Application.IServices.Product
{
    public interface IProductServices
    {
        Task<IEnumerable<Core.Entites.product.Product>> GetAllAsync(string search, int categoryId, int page, int pageSize);
        
        Task<Core.Entites.product.Product?> GetByIdAsync(int id);

        Task<Core.Entites.product.Product> CreateAsync(CreateProductDto createProductDto);

        Task<Core.Entites.product.Product> UpdateAsync(UpdateProductDto updateProductDto);
        Task<bool> DeleteAsync(int id);
    }
}
