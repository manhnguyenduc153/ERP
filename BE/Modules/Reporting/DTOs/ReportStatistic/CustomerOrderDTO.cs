namespace ERP_API.DTOS.ReportStatistic
{        public class CustomerOrderDTO
    {
        public int SalesOrderId { get; set; }
        public string OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CustomerOrderDetailDTO> OrderDetails { get; set; }
    }
}
