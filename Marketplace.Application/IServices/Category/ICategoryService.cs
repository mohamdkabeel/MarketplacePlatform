using Marketplace.Application.Dtos.Categories;
using System.Threading.Tasks;
namespace Marketplace.Application.IServices.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<Core.Entites.category.Category>> GetAllAsync();
        Task<Core.Entites.category.Category> GetByIdAsync(int id);
        Task<Core.Entites.category.Category> CreateCategoryAsync(CreateCategoryDto dto);
        Task<Core.Entites.category.Category> UpdateCategoryAsync(UpdateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(int id); 

    }
}
