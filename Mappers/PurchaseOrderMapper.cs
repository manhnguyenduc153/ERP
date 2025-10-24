using ERP_API.DTOS.PurchaseOrder;
using ERP_API.Entities;

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


    }
}
