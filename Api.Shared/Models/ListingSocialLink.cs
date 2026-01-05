using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingSocialLink
{
    public int ListingSocialLinkId { get; set; }

    public int ListingId { get; set; }

    public string NetworkName { get; set; } = null!;

    public string ProfileUrl { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedByUserId { get; set; }
}
