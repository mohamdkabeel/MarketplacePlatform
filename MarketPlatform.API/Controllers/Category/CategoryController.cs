using Marketplace.Application.Dtos.Categories;
using Marketplace.Application.IServices.Category;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlatform.API.Controllers.Category
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _service.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto) => Ok(await _service.CreateCategoryAsync(dto));

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto dto) => Ok(await _service.UpdateCategoryAsync(dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => Ok(await _service.DeleteCategoryAsync(id));
    }

}
