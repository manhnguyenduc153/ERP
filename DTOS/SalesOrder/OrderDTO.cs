namespace ERP_API.DTOS.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Contact { get; set; }
        public int? StaffId { get; set; }
        public string? StaffName { get; set; }
        public List<OrderDetailDTO>? OrderDetails { get; set; }
    }
}
