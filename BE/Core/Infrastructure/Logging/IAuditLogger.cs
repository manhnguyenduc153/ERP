using ERP_API.Models;

namespace ERP_API.Core.Infrastructure.Logging
{
    public interface IAuditLogger
    {
        Task LogAsync(AuditLogEntry logEntry);
        
        Task<PagedList<AuditLogDto>> GetListPagingAsync(AuditLogSearchModel search);
    
        Task<AuditLogDto?> GetByIdAsync(Guid id);
        

        Task<byte[]> ExportToCsvAsync(AuditLogSearchModel search);
    }
    
}
