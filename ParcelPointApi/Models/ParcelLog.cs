using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class ParcelLog
{
    public Guid Id { get; set; }

    public Guid? ParcelId { get; set; }

    public string? ParcelName { get; set; }

    public string? LockerNumber { get; set; }

    public string? Status { get; set; }

    public string? Action { get; set; }

    public DateTime? ArrivedAt { get; set; }

    public DateTime? RetrievedAt { get; set; }

    public string? RetrievedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? UserId { get; set; }
}
