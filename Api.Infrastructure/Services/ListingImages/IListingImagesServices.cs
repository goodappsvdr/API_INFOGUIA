using Api.Shared.DTOs.ListingImages;

namespace Api.Infrastructure.Services.ListingImages
{
    public interface IListingImagesServices
    {
        Task<List<ListingImagesDto>> GetAllAsync();
        Task<ListingImagesDto> GetByIdAsync(int id);
        Task<ListingImagesDto> CreateAsync(string userId, AddListingImagesDTO dto);
        Task<ListingImagesDto> UpdateAsync(string userId, UpdateListingImagesDTO dto);
    }
}
