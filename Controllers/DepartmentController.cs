using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(
            ILogger<DepartmentController> logger,
            IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
        }

      
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var result = await _departmentService.GetDepartmentsAsync();
            
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}