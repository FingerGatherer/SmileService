using System;
using System.Collections.Generic;

namespace SmileService.Models;

public partial class OrderPart
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int PartId { get; set; }

    public int Quantity { get; set; }

    public DateTime WriteOffDate { get; set; }

    public int? MasterId { get; set; }

    public virtual User? Master { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Part Part { get; set; } = null!;
}
