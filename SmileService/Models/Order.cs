using System;
using System.Collections.Generic;

namespace SmileService.Models;

public partial class Order
{
    public int Id { get; set; }

    public int DeviceId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? FinishedDate { get; set; }

    public int? MasterId { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual Device Device { get; set; } = null!;

    public virtual User? Master { get; set; }

    public virtual ICollection<OrderPart> OrderParts { get; set; } = new List<OrderPart>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
