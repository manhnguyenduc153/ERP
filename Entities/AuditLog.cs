using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_API.Entities
{
    [Table("AuditLog")]
    public class AuditLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // User context
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserRoles { get; set; } // JSON array of roles

        // Action
        [Required]
        public string Action { get; set; } = string.Empty; // INSERT, UPDATE, DELETE, LOGIN, etc.
        [Required]
        public string EntityType { get; set; } = string.Empty;
        public string? EntityId { get; set; }

        // Data snapshots
        public string? Old { get; set; } // JSON
        public string? New { get; set; } // JSON
        public string? Changes { get; set; } // JSON delta
    }
}
