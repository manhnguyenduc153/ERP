using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.DTOS.Order;
using ERP_API.Entities;

namespace ERP_API.Mappers
{
    public static class SalesOrderMapper
    {
        public static SalesOrder ToCreateEntity(this CreateSalesOrderDTO createOrderDTO)
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

        public static OrderDTO ToDto(this SalesOrder salesOrder)
        {
            var orderDto = new OrderDTO
            {
                OrderId = salesOrder.SalesOrderId,
                OrderDate = salesOrder.OrderDate,
                CustomerId = salesOrder.Customer!.CustomerId,
                CustomerName = salesOrder.Customer?.Name ?? string.Empty,
                Contact = salesOrder.Customer?.Contact ?? string.Empty,
                StaffId = salesOrder.StaffId,
                StaffName = salesOrder.Staff?.Staff.FullName ?? string.Empty,
                OrderDetails = salesOrder.SalesOrderDetails.Select(sod => new OrderDetailDTO
                {
                    DetailId = sod.DetailId,
                    ProductId = sod.ProductId ?? 0,
                    ProductName = sod.Product?.ProductName ?? string.Empty,
                    Quantity = sod.Quantity ?? 0,
                    UnitPrice = sod.UnitPrice ?? 0m
                }).ToList()
            };
            return orderDto;
        }
    }
}