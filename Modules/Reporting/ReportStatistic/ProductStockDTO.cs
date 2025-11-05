namespace ERP_API.DTOS.ReportStatistic
{
    public class ProductStockDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int QuantityImport { get; set; }
        public int QuantityExport { get; set; }
        public int CurrentStock { get; set; }
        public int DamagedQuantity { get; set; }
        public decimal TotalValue { get; set; }
    }   
}
