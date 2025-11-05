namespace ERP_API.DTOS.ReportStatistic{
      public class WarehouseStatisticRequestDTO
      {
            public int? WarehouseId { get; set; }
            public DateTime? FromDate { get; set; }
            public DateTime? ToDate { get; set; }
            public int? ProductId { get; set; }
      }
    }
