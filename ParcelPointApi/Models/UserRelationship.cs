using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

[Table("USER_RELATIONSHIP")]
public partial class UserRelationship
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_by")]
    public Guid? ModifiedBy { get; set; }

    [InverseProperty("Relationship")]
    public virtual ICollection<UserGroupMember> UserGroupMembers { get; set; } = new List<UserGroupMember>();
}
