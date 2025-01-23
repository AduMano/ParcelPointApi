using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

[Table("USER_GROUP_MEMBERS")]
public partial class UserGroupMember
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("member_id")]
    public Guid? MemberId { get; set; }

    [Column("group_id")]
    public Guid? GroupId { get; set; }

    [Column("relationship_id")]
    public Guid? RelationshipId { get; set; }

    [Column("is_authorized")]
    public bool? IsAuthorized { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_by")]
    public Guid? ModifiedBy { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("UserGroupMembers")]
    public virtual UserGroup? Group { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("UserGroupMembers")]
    public virtual User? Member { get; set; }

    [ForeignKey("RelationshipId")]
    [InverseProperty("UserGroupMembers")]
    public virtual UserRelationship? Relationship { get; set; }
}
