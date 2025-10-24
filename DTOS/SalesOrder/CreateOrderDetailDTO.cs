
namespace ERP_API.DTOS.Order
{
    public class SalesOrderDetailDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}