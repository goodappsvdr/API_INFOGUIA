using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingTag
{
    public int ListingId { get; set; }

    public int TagId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedByUserId { get; set; }
}
