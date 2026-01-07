using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.Listings
{
    public class ListingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string LogoUrl { get; set; } 

        public string Address { get; set; }
        public bool IsActive { get; set; }
    }

    public class AddListingDTO
    {
  
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


    }

    public class UpdateListingDTO 
    {
        public int Id { get; set; }
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

    }
}
