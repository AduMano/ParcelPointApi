using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

[Table("USERBIO_FP")]
public partial class UserbioFp
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("fingerprint_data")]
    public byte[]? FingerprintData { get; set; }

    [Column("fingerprint_key")]
    [StringLength(100)]
    [Unicode(false)]
    public string? FingerprintKey { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_by")]
    public Guid? ModifiedBy { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }
}
