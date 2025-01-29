using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class UserLog
{
    public Guid Id { get; set; }

    public string? Action { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? UserId { get; set; }

    public virtual User? User { get; set; }
}
