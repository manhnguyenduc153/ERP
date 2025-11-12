using System;
using System.Collections.Generic;

namespace ERP_API.Core.Database.Entities;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    public string? WarehouseName { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<PurchaseStaff> PurchaseStaffs { get; set; } = new List<PurchaseStaff>();

    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
}
