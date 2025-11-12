using System.Security.Claims;
using ERP_API.Core.Infrastructure.Logging;
using ERP_API.Models;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogger _auditLogger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogService(
            IAuditLogger auditLogger,
            IHttpContextAccessor httpContextAccessor)
        {
            _auditLogger = auditLogger ?? throw new ArgumentNullException(nameof(auditLogger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public async Task LogAsync(string action, string endpoint, object? oldValue = null, object? newValue = null)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    return;

                httpContext.Response.OnCompleted(async () =>
                {
                    try
                    {
                        var statusCode = httpContext.Response.StatusCode;
                        var userId = httpContext.User
                            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        string status = statusCode >= 200 && statusCode < 300 ? "SUCCESS" : "FAILED";

                        var logEntry = new AuditLogEntry
                        {
                            Action = action,
                            Endpoint = endpoint,
                            UserId = userId,
                            OldValue = oldValue,
                            NewValue = newValue,
                            Status = status,
                            StatusCode = statusCode,
                            CreatedAt = DateTime.UtcNow
                        };

                        using (var scope = httpContext.RequestServices.CreateScope())
                        {
                            var scopedLogger = scope.ServiceProvider.GetRequiredService<IAuditLogger>();
                            await scopedLogger.LogAsync(logEntry);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[AUDIT_LOG_ERROR] Failed to write audit log: {ex.Message}");
                    }
                });

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AUDIT_LOG_ERROR] Failed to setup audit log: {ex.Message}");
            }
        }
        public async Task<PagedList<AuditLogDto>> GetListPaging(AuditLogSearchModel search)
        {
            // Delegate to the audit logger
            return await _auditLogger.GetListPagingAsync(search);
        }
        public async Task<AuditLogDto?> GetByIdAsync(Guid id)
        {
            // Delegate to the audit logger
            return await _auditLogger.GetByIdAsync(id);
        }
        public async Task<byte[]> ExportToCsvAsync(AuditLogSearchModel search)
        {
            // Delegate to the audit logger
            return await _auditLogger.ExportToCsvAsync(search);
        }
    }
}
