using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class Gender
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual ICollection<UserInformation> UserInformations { get; set; } = new List<UserInformation>();
}
