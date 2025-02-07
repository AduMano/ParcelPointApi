using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class IncomingParcel
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }
}
