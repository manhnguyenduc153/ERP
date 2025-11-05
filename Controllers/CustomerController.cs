using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP_API.Models;
using ERP_API.Services.IServices;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService CustomerService)
        {
            _logger = logger;
            _customerService = CustomerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CustomerSearchModel model)
        {
            var result = await _customerService.GetListPaging(model);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _customerService.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CustomerSaveModel model)
        {
            var result = await _customerService.Insert(model);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(CustomerSaveModel model)
        {
            var result = await _customerService.Update(model);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _customerService.Delete(id);

            return Ok(result);
        }

        [HttpGet("v2")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }
    }
}
