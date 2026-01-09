using Marketplace.Application.Dtos.Categories;
using Marketplace.Application.IServices.Category;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Infrastructure.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly Data.ApplicationDbContext _applicationDb;

        public CategoryService(Data.ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }
        public async Task<IEnumerable<Core.Entites.category.Category>> GetAllAsync()
        {
            return await _applicationDb.categories.Include(p=> p.Products).ToListAsync();
            
        }
        public async Task<Core.Entites.category.Category?> GetByIdAsync(int id)
        {
            return await _applicationDb.categories.Include(p=>p.Products).FirstOrDefaultAsync(c=> c.Id == id);
        }

        public async Task<Core.Entites.category.Category> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var CategoryExist = await _applicationDb.categories.AnyAsync(c => c.Name == dto.Name & !c.IsDeleted);
            if (!CategoryExist)
            {
                throw new Exception($"Category with ID {dto.Name} does not exist.");
            }

            var category = new Core.Entites.category.Category { Name = dto.Name };
            await _applicationDb.categories.AddAsync(category);
            await _applicationDb.SaveChangesAsync();
            return category;
        }


        public async Task<Core.Entites.category.Category> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var category = await _applicationDb.categories.FindAsync(dto.Id);
            if (category == null) throw new Exception("Category not found");


           category.Name = dto.Name;
            await _applicationDb.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _applicationDb.categories.FindAsync(id);
            if (category == null) return false;

            _applicationDb.categories.Remove(category);
            await _applicationDb.SaveChangesAsync();
            return true;
        }
    }
}
