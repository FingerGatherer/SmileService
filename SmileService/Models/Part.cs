using System;
using System.Collections.Generic;

namespace SmileService.Models;

public partial class Part
{
    public int Id { get; set; }

    public string PartName { get; set; } = null!;

    public string? Sku { get; set; }

    public string? Supplier { get; set; }

    public decimal PurchasePrice { get; set; }

    public decimal SellingPrice { get; set; }

    public int StockQuantity { get; set; }

    public int MinThreshold { get; set; }

    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>();
}
