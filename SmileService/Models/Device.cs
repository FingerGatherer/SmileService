using System;
using System.Collections.Generic;

namespace SmileService.Models;

public partial class Device
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public string DeviceType { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string? SerialNumber { get; set; }

    public string? Equipment { get; set; }

    public string DefectDescription { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
