using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP_API.Models;
using ERP_API.Services.IServices;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/warehouses")]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger<WarehouseController> _logger;
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(ILogger<WarehouseController> logger, IWarehouseService WarehouseService)
        {
            _logger = logger;
            _warehouseService = WarehouseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] WarehouseSearchModel model)
        {
            var result = await _warehouseService.GetListPaging(model);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _warehouseService.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(WarehouseSaveModel model)
        {
            var result = await _warehouseService.Insert(model);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(WarehouseSaveModel model)
        {
            var result = await _warehouseService.Update(model);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _warehouseService.Delete(id);

            return Ok(result);
        }
    }
}
