using System;
using System.Collections.Generic;

namespace ParcelPointApi.Models;

public partial class EmailVerification
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string VerificationCode { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsUsed { get; set; }
}
