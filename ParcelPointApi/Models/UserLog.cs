using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

[Table("USER_LOGS")]
public partial class UserLog
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("action")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Action { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserLogs")]
    public virtual User? User { get; set; }
}
