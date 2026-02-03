using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Listing
{
    public int ListingId { get; set; }

    public int TenantId { get; set; }

    public int CategoryId { get; set; }

    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public string? LongDescription { get; set; }

    public string? LogoUrl { get; set; }

    public string? Address { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Email { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? VideoUrl { get; set; }

    public string? CatalogUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedByUserId { get; set; }
}
