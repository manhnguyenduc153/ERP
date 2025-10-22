using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP_API.Models;
using ERP_API.Services.IServices;

namespace ApiWithRoles.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _CustomerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService CustomerService)
        {
            _logger = logger;
            _CustomerService = CustomerService;
        }

        [HttpGet("get-list")]
        public async Task<IActionResult> GetList([FromQuery] CustomerSearchModel model)
        {
            var result = await _CustomerService.GetListPaging(model);

            return Ok(result);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _CustomerService.GetById(id);

            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(CustomerSaveModel model)
        {
            var result = await _CustomerService.Insert(model);

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(CustomerSaveModel model)
        {
            var result = await _CustomerService.Update(model);

            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _CustomerService.Delete(id);

            return Ok(result);
        }
    }
}
