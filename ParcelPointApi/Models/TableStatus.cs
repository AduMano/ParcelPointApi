using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class TableStatus
{
    public Guid Id { get; set; }

    public int LockerNumber { get; set; }

    public string LockerSize { get; set; } = null!;

    public Guid? OwnerId { get; set; }

    public bool IsOpen { get; set; }
}

public partial class TableStatusAdmin
{
    public int LockerNumber { get; set; }

    public string LockerSize { get; set; } = null!;

    public string OwnerName { get; set; }

    public bool IsOpen { get; set; }
}
