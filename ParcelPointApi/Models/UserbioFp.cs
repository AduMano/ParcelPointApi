using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class UserbioFp
{
    public Guid Id { get; set; }

    public byte[]? FingerprintData { get; set; }

    public string? FingerprintKey { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }

    public Guid? UserId { get; set; }
}
