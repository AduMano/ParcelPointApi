using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class UserInformation
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Suffix { get; set; }

    public DateOnly? Birthdate { get; set; }

    public string? PhotoUrl { get; set; }

    public string? Address { get; set; }

    public string? ContactNumber { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }

    public Guid? GenderId { get; set; }

    public Guid? UserId { get; set; }

    public virtual Gender? Gender { get; set; }

    public virtual User? User { get; set; }
}
