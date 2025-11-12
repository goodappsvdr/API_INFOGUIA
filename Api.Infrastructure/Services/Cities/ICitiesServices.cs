using Api.Shared.DTOs.Cities;

namespace Api.Infrastructure.Services.Cities
{
    public interface ICitiesServices
    {
        Task<List<Cities_Get>> GetAsync(int provinceId);

    }
}