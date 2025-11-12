using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? Name { get; set; }

    public string? Contact { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
}
