namespace ERP_API.DTOS.PurchaseOrder
{
    public class ViewPurchaseOrderDTO
    {
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string Contact { get; set; }
        public int? StaffId;
        public string? StaffName { get; set; }
        public List<ViewPurchaseOrderDetailDTO>? PurchaseOrderDetails { get; set; }
    }
}
