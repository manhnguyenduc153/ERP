using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.DTOS.Order;
using ERP_API.Entities;

namespace ERP_API.Mappers
{
    public static class OrderMapper
    {
        public static SalesOrder ToCreateEntity(this CreateOrderDTO createOrderDTO)
        {
            var salesOrder = new SalesOrder
            {
                CustomerId = createOrderDTO.CustomerId,
                SalesOrderDetails = createOrderDTO.OrderDetails.Select(od => new SalesOrderDetail
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                }).ToList()
            };

            return salesOrder;
        }
    }
}