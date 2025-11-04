namespace Api.Infrastructure.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly SignInManager<IdentityUserProfile> _signInManager;

        public AuthServices(SignInManager<IdentityUserProfile> signInManager)
        {
            _signInManager = signInManager;
        }
        
        public async Task<SignInResult> LoginAsync(Auth_Login login) => await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: false, lockoutOnFailure: false);
    }
}
