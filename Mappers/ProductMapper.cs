using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.DTOS.Product;
using ERP_API.Entities;

namespace ERP_API.Mappers
{
    public static class ProductMapper
    {
        public static Product ToEntity(this CreateProductDTO dto)
        {
            return new Product
            {
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                Unit = dto.Unit,
                UnitPrice = dto.UnitPrice
            };
        }

        public static Product ToEntity(this UpdateProductDTO dto)
        {
            return new Product
            {
                ProductName = dto.ProductName,
                CategoryId = dto.CategoryId,
                Unit = dto.Unit,
                UnitPrice = dto.UnitPrice
            };
        }
    }
}
