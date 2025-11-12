using ERP_API.Models;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAuditLogs([FromQuery] AuditLogSearchModel search)
        {
            var result = await _auditLogService.GetListPaging(search);
            return Ok(new ResponseData<PagedList<AuditLogDto>>
            {
                Data = result,
                Message = "Get audit logs successfully",
                Success = true,
                StatusCode = 200
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuditLogById(Guid id)
        {
            var result = await _auditLogService.GetByIdAsync(id);
            
            if (result == null)
            {
                return NotFound(new ResponseData<AuditLogDto>
                {
                    Message = "Audit log not found",
                    Success = false,
                    StatusCode = 404
                });
            }

            return Ok(new ResponseData<AuditLogDto>
            {
                Data = result,
                Message = "Get audit log successfully",
                Success = true,
                StatusCode = 200
            });
        }
        [HttpGet("export")]
        public async Task<IActionResult> ExportToCsv([FromQuery] AuditLogSearchModel search)
        {
            var csvBytes = await _auditLogService.ExportToCsvAsync(search);
            var fileName = $"AuditLogs_{DateTime.UtcNow:yyyyMMddHHmmss}.csv";

            return File(csvBytes, "text/csv", fileName);
        }
        [HttpPost("test-log")]
        public async Task<IActionResult> TestLog([FromBody] CreateAuditLogRequest request)
        {
            await _auditLogService.LogAsync(
                request.Action, 
                request.Endpoint, 
                request.Old, 
                request.New
            );

            return Ok(new ResponseData<string>
            {
                Message = "Log created successfully. UserId was automatically captured from current session.",
                Success = true,
                StatusCode = 200
            });
        }
    }
}
