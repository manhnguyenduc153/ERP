using System;
using System.Collections.Generic;

namespace ERP_API.Core.Database.Entities;

public partial class SalesOrder
{
    public int SalesOrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? CustomerId { get; set; }

    public string? Status { get; set; }

    public int? StaffId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();

    public virtual SaleStaff? Staff { get; set; }
}
