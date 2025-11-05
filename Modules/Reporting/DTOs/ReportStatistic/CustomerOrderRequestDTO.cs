using System.ComponentModel.DataAnnotations;

namespace ERP_API.DTOS.ReportStatistic
{
    public class CustomerOrderRequestDTO
    {
        public int? CustomerId { get; set; }
        
        [StringLength(50)]
        public string? Status { get; set; }
        
        public DateTime? FromDate { get; set; }
        
        public DateTime? ToDate { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be at least 1")]
        public int PageNumber { get; set; } = 1;
        
        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        public int PageSize { get; set; } = 20;
    }
}