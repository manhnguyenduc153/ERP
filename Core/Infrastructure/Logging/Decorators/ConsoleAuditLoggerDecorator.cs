namespace ERP_API.Core.Infrastructure.Logging.Decorators
{
    public class ConsoleAuditLoggerDecorator : AuditLoggerDecorator
    {
        private readonly bool _enableColors;

        public ConsoleAuditLoggerDecorator(
            IAuditLogger wrappedLogger,
            bool enableColors = true) 
            : base(wrappedLogger)
        {
            _enableColors = enableColors;
        }

        public override async Task LogAsync(AuditLogEntry logEntry)
        {
            try
            {
                var timestamp = logEntry.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                var userId = logEntry.UserId ?? "Anonymous";
                var statusIcon = logEntry.Status == "SUCCESS" ? "✓" : "✗";

                if (_enableColors)
                {
                    Console.ForegroundColor = logEntry.Status == "SUCCESS"
                        ? ConsoleColor.Green
                        : ConsoleColor.Red;
                }

                Console.WriteLine($"[{timestamp}] {statusIcon} [{logEntry.Status}] User: {userId} | Action: {logEntry.Action} | Endpoint: {logEntry.Endpoint}");

                if (_enableColors)
                {
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CONSOLE_DECORATOR_ERROR] {ex.Message}");
            }
            await base.LogAsync(logEntry);
        }
    }
}
