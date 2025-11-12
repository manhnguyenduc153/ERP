using System;
using System.Collections.Generic;

namespace ERP_API.Core.Database.Entities;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public string? Contact { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
