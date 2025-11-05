using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class PurchaseStaff
{
    public int StaffId { get; set; }

    public int? WarehouseId { get; set; }

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual Employee Staff { get; set; } = null!;

    public virtual Warehouse? Warehouse { get; set; }
}
