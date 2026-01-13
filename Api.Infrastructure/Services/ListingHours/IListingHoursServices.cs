using Api.Shared.DTOs.ListingHours;

namespace Api.Infrastructure.Services.ListingHours
{
    public interface IListingHoursServices
    {
        Task<List<ListingHoursDto>> GetAllAsync();
        Task<ListingHoursDto> GetByIdAsync(int id);
        Task<ListingHoursDto> CreateAsync(string userId, AddListingHoursDTO dto);
        Task<ListingHoursDto> UpdateAsync(string userId, UpdateListingHoursDTO dto);
    }
}
