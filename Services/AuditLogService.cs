using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ErpDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditLogService(
            ErpDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task LogAsync(string action, string endpoint, object? oldValue = null, object? newValue = null)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    return;

                // Đăng ký callback với Func<Task> để handle async đúng cách
                httpContext.Response.OnCompleted(async () =>
                {
                    try
                    {
                        var statusCode = httpContext.Response.StatusCode;
                        var userId = httpContext.User
                            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        string status = statusCode >= 200 && statusCode < 300 ? "SUCCESS" : "FAILED";

                        var auditLog = new AuditLog
                        {
                            Id = Guid.NewGuid(),
                            UserId = userId,
                            Action = action,
                            Endpoint = endpoint,
                            Old = oldValue != null ? JsonSerializer.Serialize(oldValue) : null,
                            New = newValue != null ? JsonSerializer.Serialize(newValue) : null,
                            Status = status,
                            CreatedAt = DateTime.UtcNow
                        };

                        // Sử dụng DbContext riêng để tránh conflict
                        using (var scope = httpContext.RequestServices.CreateScope())
                        {
                            var dbContext = scope.ServiceProvider.GetRequiredService<ErpDbContext>();
                            await dbContext.Set<AuditLog>().AddAsync(auditLog);
                            await dbContext.SaveChangesAsync();
                            
                            Console.WriteLine($"[{status}] Log recorded: {action} - StatusCode: {statusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Failed to write audit log: {ex.Message}");
                        Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                    }
                });

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to setup audit log: {ex.Message}");
            }
        }
        public async Task<PagedList<AuditLogDto>> GetListPaging(AuditLogSearchModel search)
        {
            var query = _dbContext.Set<AuditLog>().AsQueryable();
            if (!string.IsNullOrEmpty(search.UserId))
            {
                query = query.Where(x => x.UserId == search.UserId);
            }
            if (!string.IsNullOrEmpty(search.Action))
            {
                query = query.Where(x => x.Action.Contains(search.Action));
            }
            if (!string.IsNullOrEmpty(search.Endpoint))
            {
                query = query.Where(x => x.Endpoint.Contains(search.Endpoint));
            }
            if (!string.IsNullOrEmpty(search.LogStatus))
            {
                query = query.Where(x => x.Status == search.LogStatus);
            }

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(x => 
                    x.Action.Contains(search.Keyword) ||
                    x.Endpoint.Contains(search.Keyword) ||
                    (x.UserId != null && x.UserId.Contains(search.Keyword)));
            }
            if (search.FromDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= search.FromDate.Value);
            }

            if (search.ToDate.HasValue)
            {
                var toDate = search.ToDate.Value.AddDays(1); 
                query = query.Where(x => x.CreatedAt < toDate);
            }
            var totalItems = await query.CountAsync();
            query = query.OrderByDescending(x => x.CreatedAt);
            var items = await query
                .Skip((search.PageIndex - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(x => new AuditLogDto
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    UserId = x.UserId,
                    Action = x.Action,
                    Endpoint = x.Endpoint,
                    Old = x.Old,
                    New = x.New,
                    Status = x.Status
                })
                .ToListAsync();

            return new PagedList<AuditLogDto>(items, totalItems, search.PageIndex, search.PageSize);
        }
        public async Task<AuditLogDto?> GetByIdAsync(Guid id)
        {
            var auditLog = await _dbContext.Set<AuditLog>()
                .Where(x => x.Id == id)
                .Select(x => new AuditLogDto
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    UserId = x.UserId,
                    Action = x.Action,
                    Endpoint = x.Endpoint,
                    Old = x.Old,
                    New = x.New,
                    Status = x.Status
                })
                .FirstOrDefaultAsync();

            return auditLog;
        }
        public async Task<byte[]> ExportToCsvAsync(AuditLogSearchModel search)
        {
            var query = _dbContext.Set<AuditLog>().AsQueryable();
            if (!string.IsNullOrEmpty(search.UserId))
            {
                query = query.Where(x => x.UserId == search.UserId);
            }

            if (!string.IsNullOrEmpty(search.Action))
            {
                query = query.Where(x => x.Action.Contains(search.Action));
            }

            if (!string.IsNullOrEmpty(search.Endpoint))
            {
                query = query.Where(x => x.Endpoint.Contains(search.Endpoint));
            }

            if (!string.IsNullOrEmpty(search.LogStatus))
            {
                query = query.Where(x => x.Status == search.LogStatus);
            }

            if (!string.IsNullOrEmpty(search.Keyword))
            {
                query = query.Where(x => 
                    x.Action.Contains(search.Keyword) ||
                    x.Endpoint.Contains(search.Keyword) ||
                    (x.UserId != null && x.UserId.Contains(search.Keyword)));
            }

            if (search.FromDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= search.FromDate.Value);
            }

            if (search.ToDate.HasValue)
            {
                var toDate = search.ToDate.Value.AddDays(1);
                query = query.Where(x => x.CreatedAt < toDate);
            }

            var logs = await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            var csv = new StringBuilder();
            csv.AppendLine("Datetime,UserId,Endpoint,Action,Status,OldData,NewData");

            foreach (var log in logs)
            {
                var datetime = log.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                var userId = log.UserId ?? "Anonymous";
                var endpoint = EscapeCsvValue(log.Endpoint);
                var action = EscapeCsvValue(log.Action);
                var status = log.Status;
                var oldData = EscapeCsvValue(log.Old ?? "");
                var newData = EscapeCsvValue(log.New ?? "");
                var formattedRow = $"[{datetime}],[{userId}],[{endpoint}],[{action}],[{status}],[{oldData}],[{newData}]";     
                csv.AppendLine(formattedRow);
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }
        private string EscapeCsvValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }

            return value;
        }
    }
}
