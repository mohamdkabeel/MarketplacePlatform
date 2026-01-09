using Marketplace.Application.Dtos.Products;
using Marketplace.Application.IServices.Product;
using Marketplace.Infrastructure.Services.FileService;
using Marketplace.Infrastructure.Services.Product;
//using Marketplace.Infrastructure.IServices.Prooduct;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Marketplace.Application.Dtos.Products.CreateImageProductDto;

namespace MarketPlatform.API.Controllers.Product
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _service;
        private readonly FileServices _fileServices;
        private readonly ProductService _productService;

        public ProductsController(IProductServices service , FileServices fileServices , ProductService productService )
        {
            _service = service;
            _fileServices = fileServices;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var products = await _service.GetAllAsync(search, categoryId, page, pageSize);
            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create([FromForm] CreateImageProductDto dto)
        {
            string imageUrl = "/images/products/default.jpg"; // default image

            if (dto.Image != null)
            {
                imageUrl = await _fileServices.UploadProductImageAsync(dto.Image);
            }

            var productDto = new CreateProductDto
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                ImageUrl = imageUrl
            };

            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var product = await _productService.CreateAsync(productDto);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            var product = await _service.UpdateAsync(dto);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? Ok("Deleted") : NotFound();
        }
    }

}
