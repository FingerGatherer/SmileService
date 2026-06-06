using System;
using System.Collections.Generic;

namespace SmileService.Models;

public partial class Client
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
}
