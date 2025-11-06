using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP_API.Models;
using ERP_API.Services.IServices;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService CategoryService)
        {
            _logger = logger;
            _categoryService = CategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CategorySearchModel model)
        {
            var result = await _categoryService.GetListPaging(model);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CategorySaveModel model)
        {
            var result = await _categoryService.Insert(model);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(CategorySaveModel model)
        {
            var result = await _categoryService.Update(model);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _categoryService.Delete(id);

            return Ok(result);
        }

        [HttpGet("v2")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return Ok(result);
        }
    }
}
