using Api.Shared.DTOs.Listings;

namespace Api.Infrastructure.Services.Listings
{
    public interface IListingsServices
    {
        Task<ListingDTO> CreateListingAsync(string userId, AddListingDTO listingDto);
        Task<List<ListingDTO>> GetAllListingsAsync();
        Task<ListingDTO> GetListingByIdAsync(int id);
        Task<ListingDTO> UpdateListingAsync(string userId, ListingDTO listingDto);
    }
}