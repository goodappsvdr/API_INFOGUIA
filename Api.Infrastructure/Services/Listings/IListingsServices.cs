using Api.Shared.DTOs.Listings;

namespace Api.Infrastructure.Services.Listings
{
    public interface IListingsServices
    {
        Task<ListingDTO> CreateListingAsync(int userId, AddListingDTO listingDto);
        Task<GetAllListingsResult> GetAllListingsAsync();
        Task<ListingDTO> GetListingByIdAsync(int id);
        Task<GetAllListingsByResult> GetListingsByCategoryIdAsync(int categoryId);
        Task<ListingDTO> UpdateListingAsync(int userId, UpdateListingDTO listingDto, int listingId);
    }
}