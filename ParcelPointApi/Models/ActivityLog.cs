using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class ActivityLog
{
    public Guid Id { get; set; }

    public string ActionTitle { get; set; } = null!;

    public string? ActionContext { get; set; }

    public string? Module { get; set; }

    public string? SubModule { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }
}
