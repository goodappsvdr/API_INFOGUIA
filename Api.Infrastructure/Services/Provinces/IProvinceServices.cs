using Api.Shared.DTOs.Province;

namespace Api.Infrastructure.Services.Province
{
    public interface IProvinceServices
    {
        Task<List<Province_Get>> GetAsync();
    }
}