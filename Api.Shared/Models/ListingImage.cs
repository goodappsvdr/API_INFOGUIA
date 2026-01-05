using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingImage
{
    public int ListingImageId { get; set; }

    public int ListingId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? Caption { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedByUserId { get; set; }
}
