using Api.Shared.DTOs.ListingHours;
using Api.Shared.DTOs.ListingImages;
using Api.Shared.DTOs.ListingPhones;
using Api.Shared.DTOs.ListingSocialLinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.Listings
{
    public class ListingDTO
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
        public int UserId { get; set; }

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
        public bool IsActive { get; set; }
        public int UserId { get; set; }


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
        public int UserId { get; set; }

    }

    public class GetAllListingsByResult
    {
        public int TotalCount { get; set; }
        public List<GetAllListingBy> Items { get; set; }
    }
    public class AddListingCompleteDTO
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
        public bool IsActive { get; set; }
        public int UserId { get; set; }

        public List<AddListingServicesComleteDTO> Services { get; set; }
        public List<AddListingHoursCompleteDTO> Hours { get; set; }
        public List<AddListingSocialLinksCompleteDTO> socialLinks { get; set; }
        public List<AddListingPhonesCompleteDTO> phones { get; set; }


    }

    public class UpdateListingCompleteDTO
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
        public int UserId { get; set; }

        public List<ListingServices> Services { get; set; }
        public List<AddListingHoursCompleteDTO> Hours { get; set; }
        public List<AddListingSocialLinksCompleteDTO> socialLinks { get; set; }
        public List<AddListingPhonesCompleteDTO> phones { get; set; }

    }

    public class GetAllListingBy
    {
        public int ListingId { get; set; }
        public int TenantId { get; set; }

        public int CategoryId { get; set; }
        public string Category { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

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
        public int UserId { get; set; }



    }


    public class GetAllListingsResult
    {
        public int TotalCount { get; set; }
        public List<GetAllListing> Items { get; set; }
    }


    public class GetAllListing
    {
        public int ListingId { get; set; }
        public int TenantId { get; set; }

        public int CategoryId { get; set; }
        public string Category { get; set; }

        public int CityId { get; set; }
        public string City { get; set; }

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
        public int UserId { get; set; }

        public List<GetListingServices> Services { get; set; }
        public List<GetListingHours> Hours { get; set; }
        public List<GetListingImages> Images { get; set; }
        public List<GetListingSocialLinks> socialLinks { get; set; }
        public List<GetListingPhones> phones { get; set; }


    }

}
