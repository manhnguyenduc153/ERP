   public class StockHistoryRequestDTO
    {
        public int? WarehouseId { get; set; }
        public int? ProductId { get; set; }
        public string? TransactionType { get; set; } // "IMPORT", "EXPORT", "ALL" hoặc null
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }