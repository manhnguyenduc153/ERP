using System.Text;
using System.Text.Json;
using ERP_API.Entities;
using ERP_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Core.Infrastructure.Logging
{
    public class DatabaseAuditLogger : IAuditLogger
    {
        protected readonly ErpDbContext _dbContext;

        public DatabaseAuditLogger(ErpDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual async Task LogAsync(AuditLogEntry logEntry)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    Id = Guid.NewGuid(),
                    UserId = logEntry.UserId,
                    Action = logEntry.Action,
                    Endpoint = logEntry.Endpoint,
                    Old = logEntry.OldValue != null ? JsonSerializer.Serialize(logEntry.OldValue) : null,
                    New = logEntry.NewValue != null ? JsonSerializer.Serialize(logEntry.NewValue) : null,
                    Status = logEntry.Status,
                    CreatedAt = logEntry.CreatedAt
                };

                await _dbContext.Set<AuditLog>().AddAsync(auditLog);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DATABASE_AUDIT_LOGGER_ERROR] Failed to write audit log: {ex.Message}");
            }
        }

        public virtual async Task<PagedList<AuditLogDto>> GetListPagingAsync(AuditLogSearchModel search)
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

        public virtual async Task<AuditLogDto?> GetByIdAsync(Guid id)
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

        public virtual async Task<byte[]> ExportToCsvAsync(AuditLogSearchModel search)
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

        protected string EscapeCsvValue(string value)
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
