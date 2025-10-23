using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP_API.DTOS.Product
{
    public class CreateProductDTO
    {
        public string? ProductName { get; set; }

        public int? CategoryId { get; set; }

        public string? Unit { get; set; }

        public decimal? UnitPrice { get; set; }
    }
}