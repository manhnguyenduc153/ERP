namespace ERP_API.DTOS.ReportStatistic
{
      // Request DTOs
 public class WarehouseStatisticDTO
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string Location { get; set; }
        public int TotalImport { get; set; }
        public int TotalExport { get; set; }
        public int CurrentStock { get; set; }
        public int DamagedItems { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
