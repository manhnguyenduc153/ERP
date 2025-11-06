using ERP_API.DTOS.PurchaseOrder;
using ERP_API.Core.Database.Entities;

namespace ERP_API.Mappers
{
    public static class PurchaseOrderMapper
    {
        public static PurchaseOrder ToEntity(this CreatePurchaseOrderDTO dto)
        {
            return new PurchaseOrder
            {
                SupplierId = dto.SupplierId,
                PurchaseOrderDetails = dto.PurchaseOrderDetails.Select(detail => new PurchaseOrderDetail
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };
        }

        public static ViewPurchaseOrderDTO ToViewDTO(this PurchaseOrder entity)
        {
            return new ViewPurchaseOrderDTO
            {
                OrderId = entity.PurchaseOrderId,
                OrderDate = entity.OrderDate,
                SupplierId = entity.SupplierId,
                SupplierName = entity.Supplier?.SupplierName,
                Contact = entity.Supplier?.Contact,
                StaffId = entity.StaffId,
                StaffName = entity.Staff?.Staff.FullName,
                PurchaseOrderDetails = entity.PurchaseOrderDetails?.Select(detail => new ViewPurchaseOrderDetailDTO
                {
                    DetailId = detail.DetailId,
                    ProductId = detail.Product!.ProductId,
                    ProductName = detail.Product?.ProductName ?? string.Empty,
                    Quantity = detail.Quantity,
                    UnitPrice = detail.UnitPrice
                }).ToList()
            };
        }



    }
}
