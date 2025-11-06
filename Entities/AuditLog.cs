using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class AuditLog
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? Old { get; set; }

    public string? New { get; set; }

    public string Endpoint { get; set; } = null!;

    /// <summary>
    /// SUCCESS or FAILED
    /// </summary>
    public string Status { get; set; } = null!;
}
