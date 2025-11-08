using Api.Shared.DTOs.Tenant;
using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class ListingImage : BaseEntity
{
    public int ListingImageId { get; set; }

    public int ListingId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? Caption { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }


}
