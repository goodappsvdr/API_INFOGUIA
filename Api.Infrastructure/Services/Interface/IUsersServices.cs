

using Api.Shared.DTOs.Auth;

namespace Api.Infrastructure.Services.Interface
{
    public interface IUsersServices
    {
        Task<Jwt_Claims> GetClaimsAsync(string username);
        Task<bool> LoginAsync(Auth_Login login);
    }
}
