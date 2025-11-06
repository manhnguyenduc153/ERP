using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class PurchaseOrderDetail
{
    public int DetailId { get; set; }

    public int? PurchaseOrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual Product? Product { get; set; }

    public virtual PurchaseOrder? PurchaseOrder { get; set; }
}
