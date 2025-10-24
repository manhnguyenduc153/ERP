namespace ERP_API.DTOS.Order
{
    public class OrderDetailDTO
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
