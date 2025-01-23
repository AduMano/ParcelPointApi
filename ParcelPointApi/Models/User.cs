using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

[Table("USERS")]
[Index("Username", Name = "UQ__USERS__F3DBC57296B360BE", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("username")]
    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [Column("password")]
    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_by")]
    public Guid? ModifiedBy { get; set; }

    [Column("role_id")]
    public Guid? RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role? Role { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<UserGroupMember> UserGroupMembers { get; set; } = new List<UserGroupMember>();

    [InverseProperty("Owner")]
    public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();

    [InverseProperty("User")]
    public virtual ICollection<UserInformation> UserInformations { get; set; } = new List<UserInformation>();

    [InverseProperty("User")]
    public virtual ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();
}
