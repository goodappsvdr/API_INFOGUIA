using Api.Shared.DTOs.Politics;

namespace Api.Shared.Interface
{
    public interface IPoliticsServices
    {
        Task<Politic_Get> GetAsync(string Type);
        Task<bool> UpdateAsync(Politic_Update Model);
    }
}