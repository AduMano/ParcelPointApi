using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

[Table("ACTIVITY_LOGS")]
public partial class ActivityLog
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("action_title")]
    [StringLength(255)]
    [Unicode(false)]
    public string ActionTitle { get; set; } = null!;

    [Column("action_context", TypeName = "text")]
    public string? ActionContext { get; set; }

    [Column("module")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Module { get; set; }

    [Column("sub_module")]
    [StringLength(50)]
    [Unicode(false)]
    public string? SubModule { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }
}
