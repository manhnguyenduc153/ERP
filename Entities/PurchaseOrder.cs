using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? SupplierId { get; set; }

    public string? Status { get; set; }

    public int? StaffId { get; set; }

    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    public virtual PurchaseStaff? Staff { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
