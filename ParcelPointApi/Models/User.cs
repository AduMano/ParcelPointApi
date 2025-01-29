using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }

    public Guid? RoleId { get; set; }

    public bool IsActive { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<UserGroupMember> UserGroupMembers { get; set; } = new List<UserGroupMember>();

    public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();

    public virtual ICollection<UserInformation> UserInformations { get; set; } = new List<UserInformation>();

    public virtual ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();
}
