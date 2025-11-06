namespace ERP_API.Models
{
    public class AuditLogSearchModel : BaseSearch
    {
        public string? UserId { get; set; }
        public string? Action { get; set; }
        public string? Endpoint { get; set; }
        public string? LogStatus { get; set; } 
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class AuditLogDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string Action { get; set; } = null!;
        public string? Old { get; set; }
        public string? New { get; set; }
        public string Endpoint { get; set; } = null!;
        public string Status { get; set; } = null!; 
            public string FormattedLog => 
            $"[{CreatedAt:yyyy-MM-dd HH:mm:ss}] - [{UserId ?? "Anonymous"}] - [{Endpoint}] - [{Action}] - [{Status}] - [Old: {Old ?? "null"}, New: {New ?? "null"}]";
    }

    public class CreateAuditLogRequest
    {
        public string Action { get; set; } = null!;
        public string Endpoint { get; set; } = null!;
        public object? Old { get; set; }
        public object? New { get; set; }
    }
}
