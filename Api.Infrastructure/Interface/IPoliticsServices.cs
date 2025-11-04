namespace Api.Infrastructure.Services.Interface
{
    public interface IPoliticsServices
    {
        Task<Politic_Get> GetAsync(string Type);
        Task<bool> UpdateAsync(Politic_Update Model);
    }
}