using System;
using Marketplace.Application.Dtos.Products;
using Marketplace.Application.IServices.Product;
using Marketplace.Application.Specifications.Product;
using Marketplace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Marketplace.Infrastructure.Services.Product
{
    public class ProductService : IProductServices
    {
        private readonly ApplicationDbContext _applicationDb;

        public ProductService(ApplicationDbContext applicationDb)
        {
            _applicationDb = applicationDb;
        }

        //public async Task<IEnumerable<Core.Entites.product.Product>> GetAllAsync(string? search = null, int? categoryId = null, int page = 1, int pageSize = 10)
        //{
        //    var spec = new ProductSpecification(search, categoryId);
        //    var query = _applicationDb.products.Include(p => p.Category).AsQueryable();

        //    if (spec.Criteria != null)
        //        query = query.Where(spec.Criteria);

        //    return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        //}

        public async Task<Core.Entites.product.Product?> GetByIdAsync(int id)
        {
            return await _applicationDb.products.FindAsync(id);
        }

        public async Task<Core.Entites.product.Product> CreateAsync(CreateProductDto createProductDto)
        {
            var CategoryExist = await _applicationDb.categories.AnyAsync(p => p.Id == createProductDto.CategoryId && !p.IsDeleted);

            if (!CategoryExist)
            {
                throw new Exception($"Category with ID {createProductDto.CategoryId} does not exist.");
            }

            var product = new Core.Entites.product.Product()
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                ImageUrl = createProductDto.ImageUrl,
                CategoryId = createProductDto.CategoryId,
                OwnerId = createProductDto.OwnerId
                
            };

            _applicationDb.products.Add(product);

            await _applicationDb.SaveChangesAsync();

            return product;
        }

        public async Task<Core.Entites.product.Product> UpdateAsync(UpdateProductDto updateProductDto)
        {
            var product = await _applicationDb.products.FindAsync(updateProductDto.Id);

            if (product == null) throw new Exception("Product not found");

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;
            product.ImageUrl = updateProductDto.ImageUrl;
            product.CategoryId = updateProductDto.CategoryId;

            await _applicationDb.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _applicationDb.products.FindAsync(id);

            if (product == null) return false;

            _applicationDb.products.Remove(product);
            await _applicationDb.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Core.Entites.product.Product>> GetAllAsync(string search, int categoryId, int page, int pageSize)
        {
            var spec = new ProductSpecification(search, categoryId);
            var query = _applicationDb.products.Include(p => p.Category).AsQueryable();

            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
    }
}
