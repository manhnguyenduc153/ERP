using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;

namespace ERP_API.DTOS.Order
{
    public class CreateOrderDTO
    {
        public int CustomerId { get; set; }
        public List<CreateOrderDetailDTO> OrderDetails { get; set; } = new List<CreateOrderDetailDTO>();
    }
}