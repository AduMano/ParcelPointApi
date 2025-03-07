using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class SystemMode
{
    public int Id { get; set; }

    public string? CurrentState { get; set; }

    public DateTime? LastUpdate { get; set; }

    public int? BiometricId { get; set; }
}
