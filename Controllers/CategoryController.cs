using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP_API.Models;
using ERP_API.Services.IServices;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _CategoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService CategoryService)
        {
            _logger = logger;
            _CategoryService = CategoryService;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetList([FromQuery] CategorySearchModel model)
        {
            var result = await _CategoryService.GetListPaging(model);

            return Ok(result);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _CategoryService.GetById(id);

            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(CategorySaveModel model)
        {
            var result = await _CategoryService.Insert(model);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(CategorySaveModel model)
        {
            var result = await _CategoryService.Update(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _CategoryService.Delete(id);

            return Ok(result);
        }
    }
}
