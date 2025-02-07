using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class UserGroupMember
{
    public Guid Id { get; set; }

    public Guid? MemberId { get; set; }

    public Guid? GroupId { get; set; }

    public Guid? RelationshipId { get; set; }

    public bool? IsAuthorized { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }

    public virtual UserGroup? Group { get; set; }

    public virtual User? Member { get; set; }

    public virtual UserRelationship? Relationship { get; set; }
}
