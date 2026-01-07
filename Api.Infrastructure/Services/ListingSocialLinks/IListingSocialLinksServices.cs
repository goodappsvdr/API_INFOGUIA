using Api.Shared.DTOs.ListingSocialLinks;

namespace Api.Infrastructure.Services.ListingSocialLinks
{
    public interface IListingSocialLinksServices
    {
        // CREATE
        Task<ListingSocialLinksDto> CreateAsync(
            string userId,
            AddListingSocialLinksDTO dto);

        // READ
        Task<List<ListingSocialLinksDto>> GetByListingIdAsync(int listingId);
        Task<ListingSocialLinksDto> GetByIdAsync(int id);

        // UPDATE
        Task<ListingSocialLinksDto> UpdateAsync(
            string userId,
            UpdateListingSocialLinksDTO dto);

        // DELETE (soft)
        Task DeleteAsync(string userId, int id);
    }
}
