using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class UserbioTemp
{
    public Guid? Id { get; set; }

    public int BioId { get; set; }

    public Guid OwnerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }
}
