using Api.Shared.DTOs.ListingPhones;

namespace Api.Infrastructure.Services.ListingPhones
{
    public interface IListingPhonesServices
    {
        Task<List<ListingPhonesDto>> GetAllAsync();
        Task<ListingPhonesDto?> GetByIdAsync(int id);
        Task<ListingPhonesDto> CreateAsync(string userId, AddListingPhonesDTO dto);
        Task<ListingPhonesDto> UpdateAsync(string userId, UpdateListingPhonesDTO dto);
    }
}
