using Api.Shared.DTOs.ListingServices;

namespace Api.Infrastructure.Services.ListingServices
{
    public interface IListingServicesService
    {
        // CREATE
        Task<ListingServicesDto> CreateAsync(
            string userId,
            AddListingServicesDTO dto);

        // READ
        Task<List<ListingServicesDto>> GetByListingIdAsync(int listingId);

        // DELETE
        Task DeleteAsync(
            string userId,
            int listingId,
            int serviceId);
    }
}
