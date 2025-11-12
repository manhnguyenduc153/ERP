namespace ERP_API.Models
{
    public class BaseSearch
    {
        public string? Keyword { get; set; }
        public int? Status { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
