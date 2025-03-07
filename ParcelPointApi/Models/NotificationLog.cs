using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class NotificationLog
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Context { get; set; } = null!;

    public int? LockerNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsRead { get; set; }

    public string? RetrievedBy { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
