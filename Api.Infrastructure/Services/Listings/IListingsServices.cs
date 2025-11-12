using Api.Shared.Models;

namespace Api.Infrastructure.Services.Listings
{
    public interface IListingsServices
    {
        Task<List<Listing>> GetAllListingsAsync();
    }
}
