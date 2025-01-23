using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

[Table("USER_INFORMATION")]
public partial class UserInformation
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("first_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? FirstName { get; set; }

    [Column("middle_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? MiddleName { get; set; }

    [Column("last_name")]
    [StringLength(50)]
    [Unicode(false)]
    public string? LastName { get; set; }

    [Column("suffix")]
    [StringLength(10)]
    [Unicode(false)]
    public string? Suffix { get; set; }

    [Column("birthdate")]
    public DateOnly? Birthdate { get; set; }

    [Column("address", TypeName = "text")]
    public string? Address { get; set; }

    [Column("contact_number")]
    [StringLength(20)]
    [Unicode(false)]
    public string? ContactNumber { get; set; }

    [Column("email")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Email { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("created_by")]
    public Guid? CreatedBy { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_by")]
    public Guid? ModifiedBy { get; set; }

    [Column("gender_id")]
    public Guid? GenderId { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [ForeignKey("GenderId")]
    [InverseProperty("UserInformations")]
    public virtual Gender? Gender { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserInformations")]
    public virtual User? User { get; set; }
}
