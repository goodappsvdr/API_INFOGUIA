using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingPhone
{
    public int ListingPhoneId { get; set; }

    public int ListingId { get; set; }

    public string PhoneType { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedByUserId { get; set; }
}
