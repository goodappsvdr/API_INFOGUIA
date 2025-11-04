namespace Api.Infrastructure.Services.Interface
{
    public interface IAuthServices
    {
        Task<SignInResult> LoginAsync(Auth_Login login);
    }
}
