using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;

namespace ERP_API.DTOS.Order
{
    public class CreateOrderDTO
    {
        public int CustomerId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Contact { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public List<CreateOrderDetailDTO> OrderDetails { get; set; } = new List<CreateOrderDetailDTO>();
    }
}