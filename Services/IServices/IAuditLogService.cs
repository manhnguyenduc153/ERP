using ERP_API.Models;

namespace ERP_API.Services.IServices
{
    public interface IAuditLogService
    {
        Task LogAsync(string action, string endpoint, object? oldValue = null, object? newValue = null);
        Task<PagedList<AuditLogDto>> GetListPaging(AuditLogSearchModel search);
        Task<AuditLogDto?> GetByIdAsync(Guid id);
        Task<byte[]> ExportToCsvAsync(AuditLogSearchModel search);
    }
}
