using ERP_API.Models;

namespace ERP_API.Core.Infrastructure.Logging
{
    public abstract class AuditLoggerDecorator : IAuditLogger
    {
        protected readonly IAuditLogger _wrappedLogger;

        protected AuditLoggerDecorator(IAuditLogger wrappedLogger)
        {
            _wrappedLogger = wrappedLogger ?? throw new ArgumentNullException(nameof(wrappedLogger));
        }
        public virtual async Task LogAsync(AuditLogEntry logEntry)
        {
            await _wrappedLogger.LogAsync(logEntry);
        }
        public virtual async Task<PagedList<AuditLogDto>> GetListPagingAsync(AuditLogSearchModel search)
        {
            return await _wrappedLogger.GetListPagingAsync(search);
        }
        public virtual async Task<AuditLogDto?> GetByIdAsync(Guid id)
        {
            return await _wrappedLogger.GetByIdAsync(id);
        }
        public virtual async Task<byte[]> ExportToCsvAsync(AuditLogSearchModel search)
        {
            return await _wrappedLogger.ExportToCsvAsync(search);
        }
    }
}
