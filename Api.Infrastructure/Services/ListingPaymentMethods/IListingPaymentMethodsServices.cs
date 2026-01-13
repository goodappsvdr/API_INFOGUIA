using Api.Shared.DTOs.ListingPaymentMethods;

namespace Api.Infrastructure.Services.ListingPaymentMethods
{
    public interface IListingPaymentMethodsServices
    {
        Task<List<ListingPaymentMethodsDto>> GetAllAsync();
        Task<ListingPaymentMethodsDto> GetByIdAsync(int listingId, int paymentMethodId);
        Task<ListingPaymentMethodsDto> CreateAsync(string userId, AddListingPaymentMethodsDTO dto);
        Task<ListingPaymentMethodsDto> UpdateAsync(string userId, UpdateListingPaymentMethodsDTO dto);
    }
}
