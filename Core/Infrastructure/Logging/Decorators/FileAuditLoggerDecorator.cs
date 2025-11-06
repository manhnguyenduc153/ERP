using System.Text;
using System.Text.Json;

namespace ERP_API.Core.Infrastructure.Logging.Decorators
{

    public class FileAuditLoggerDecorator : AuditLoggerDecorator
    {
        private readonly string _logDirectory;
        private readonly string _logFilePrefix;
        private static readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

        public FileAuditLoggerDecorator(
            IAuditLogger wrappedLogger,
            string? logDirectory = null,
            string? logFilePrefix = "audit") 
            : base(wrappedLogger)
        {
            _logDirectory = logDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), "Logs", "Audit");
            _logFilePrefix = logFilePrefix ?? "audit";
            
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        public override async Task LogAsync(AuditLogEntry logEntry)
        {
            try
            {
                var logFileName = $"{_logFilePrefix}_{DateTime.UtcNow:yyyyMMdd}.log";
                var logFilePath = Path.Combine(_logDirectory, logFileName);

                var logLine = FormatLogEntry(logEntry);

                await _fileLock.WaitAsync();
                try
                {
                    await File.AppendAllTextAsync(logFilePath, logLine + Environment.NewLine, Encoding.UTF8);
                }
                finally
                {
                    _fileLock.Release();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FILE_DECORATOR_ERROR] Failed to write to file: {ex.Message}");
            }
            await base.LogAsync(logEntry);
        }

        private string FormatLogEntry(AuditLogEntry logEntry)
        {
            var sb = new StringBuilder();
            sb.Append($"[{logEntry.CreatedAt:yyyy-MM-dd HH:mm:ss}] ");
            sb.Append($"[{logEntry.Status}] ");
            sb.Append($"User: {logEntry.UserId ?? "Anonymous"} | ");
            sb.Append($"Action: {logEntry.Action} | ");
            sb.Append($"Endpoint: {logEntry.Endpoint}");
            
            if (logEntry.OldValue != null)
            {
                sb.Append($" | Old: {JsonSerializer.Serialize(logEntry.OldValue)}");
            }
            
            if (logEntry.NewValue != null)
            {
                sb.Append($" | New: {JsonSerializer.Serialize(logEntry.NewValue)}");
            }

            return sb.ToString();
        }
    }
}
