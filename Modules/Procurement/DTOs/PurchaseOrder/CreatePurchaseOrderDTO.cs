
namespace ERP_API.DTOS.PurchaseOrder
{
    public class CreatePurchaseOrderDTO
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;

        public List<PurchaseOrderDetailDTO> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetailDTO>();
    }
}
