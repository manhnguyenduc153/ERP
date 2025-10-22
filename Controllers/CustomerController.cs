using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP_API.Models;
using ERP_API.Services.IServices;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierService _SupplierService;

        public SupplierController(ILogger<SupplierController> logger, ISupplierService SupplierService)
        {
            _logger = logger;
            _SupplierService = SupplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] SupplierSearchModel model)
        {
            var result = await _SupplierService.GetListPaging(model);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _SupplierService.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(SupplierSaveModel model)
        {
            var result = await _SupplierService.Insert(model);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(SupplierSaveModel model)
        {
            var result = await _SupplierService.Update(model);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _SupplierService.Delete(id);

            return Ok(result);
        }
    }
}
