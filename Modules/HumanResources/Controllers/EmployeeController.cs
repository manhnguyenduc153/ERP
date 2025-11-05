using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(
            ILogger<EmployeeController> logger,
            IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

   
        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeListRequestDTO request)
        {
            var result = await _employeeService.GetEmployeesAsync(request);
            return Ok(new
            {
                success = true,
                message = "Employees retrieved successfully",
                data = result.Data,
                metaData = new
                {
                    totalItems = result.TotalRecords,
                    pageIndex = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages,
                    hasNext = result.HasNext,
                    hasPrevious = result.HasPrevious
                }
            });
        }

    
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetEmployeeById(int employeeId)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);
            if (result == null)
                return NotFound(new { success = false, message = "Employee not found" });

            return Ok(new
            {
                success = true,
                message = "Employee retrieved successfully",
                data = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDTO dto)
        {
            var result = await _employeeService.CreateEmployeeAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetEmployeeById), new { employeeId = result.Data }, result);
        }

   
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, [FromBody] UpdateEmployeeDTO dto)
        {
            if (employeeId != dto.EmployeeId)
                return BadRequest(new { success = false, message = "Employee ID mismatch" });

            var result = await _employeeService.UpdateEmployeeAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            var result = await _employeeService.DeleteEmployeeAsync(employeeId);
            return Ok(result);
        }

        
        [HttpGet("report/salary-position")]
        public async Task<IActionResult> GetEmployeeReport([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var result = await _employeeService.GetEmployeeReportAsync(fromDate, toDate);
            return Ok(result);
        }
    }
}