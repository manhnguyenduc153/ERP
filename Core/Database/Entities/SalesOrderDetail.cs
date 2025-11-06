using System;
using System.Collections.Generic;

namespace ERP_API.Core.Database.Entities;

public partial class SalesOrderDetail
{
    public int DetailId { get; set; }

    public int? SalesOrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public virtual Product? Product { get; set; }

    public virtual SalesOrder? SalesOrder { get; set; }
}
