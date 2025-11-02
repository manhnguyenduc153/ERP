namespace ERP_API.DTOS.PurchaseOrder
{
    public class ViewPurchaseOrderDetailDTO
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
