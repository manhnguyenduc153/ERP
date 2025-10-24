
namespace ERP_API.DTOS.PurchaseOrder
{
    public class PurchaseOrderDetailDTO
    {

        // Exising product or new product
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        // Import details
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
    }
}