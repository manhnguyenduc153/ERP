public class StockTransactionHistoryDTO
    {
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string TransactionType { get; set; } 
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        
        public decimal UnitPrice { get; set; } 
        public decimal TotalValue { get; set; } 
    }