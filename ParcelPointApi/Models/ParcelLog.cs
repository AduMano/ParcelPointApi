using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

[Table("PARCEL_LOGS")]
public partial class ParcelLog
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("parcel_id")]
    public Guid? ParcelId { get; set; }

    [Column("parcel_name")]
    [StringLength(100)]
    [Unicode(false)]
    public string? ParcelName { get; set; }

    [Column("locker_number")]
    [StringLength(50)]
    [Unicode(false)]
    public string? LockerNumber { get; set; }

    [Column("status")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Status { get; set; }

    [Column("action")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Action { get; set; }

    [Column("arrived_at")]
    public DateTime? ArrivedAt { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }
}
