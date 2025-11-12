using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class StockTransaction
{
    public int TransactionId { get; set; }

    public int? ProductId { get; set; }

    public int? WarehouseId { get; set; }

    public int? Quantity { get; set; }

    public string? TransactionType { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}
