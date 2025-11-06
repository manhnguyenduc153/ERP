namespace ERP_API.Models
{
    public class AuditLogEntry
    {
        public string Action { get; set; } = null!;
        public string Endpoint { get; set; } = null!;
        public string? UserId { get; set; }
        public object? OldValue { get; set; }
        public object? NewValue { get; set; }
        public string Status { get; set; } = "SUCCESS";
        public int StatusCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}