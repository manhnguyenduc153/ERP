namespace ERP_API.DTOS.ReportStatistic
{
    public class DashboardSummaryDTO
    {
        public int TotalWarehouses { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }
}