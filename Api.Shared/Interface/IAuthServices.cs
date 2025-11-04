using Api.Shared.DTOs.Auth;

namespace Api.Shared.Interface
{
    public interface IAuthServices
    {
        Task<SignInResult> LoginAsync(Auth_Login login);
    }
}
