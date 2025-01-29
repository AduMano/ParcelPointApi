using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class UserGroup
{
    public Guid Id { get; set; }

    public Guid? OwnerId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual User? Owner { get; set; }

    public virtual ICollection<UserGroupMember> UserGroupMembers { get; set; } = new List<UserGroupMember>();
}
