using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.Listing
{
    public class Listing_input
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
        public bool IsActive { get; set; } = true;
        public string? CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
    }

    public class Listing_Get
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
        public bool IsActive { get; set; } = true;
        public string? CreatedByUserId { get; set; }
        public string? ModifiedByUserId { get; set; }
    }

}
