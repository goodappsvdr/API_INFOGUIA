
using Api.Shared.DTOs.Auth;
using Api.Shared.DTOs.Identity.AspNetUsers;
using Api.Shared.DTOs.Identity.Roles;
using Api.Shared.Identity;

namespace Api.Shared.Interface
{
    public interface IIdentityRepository<T> : IRepository<T> where T : class
    {
        Task<IdentityResult> AddRoleAsync(IdentityRole Model);
        Task<IdentityResult> AddRoleToUser(IdentityUserProfile Model, Role_Create role_Create);
        Task<IdentityResult> AddUserAsync(IdentityUserProfile Model, string Password);
        Task<IdentityResult> ChangePasswordAsync(IdentityUserProfile User, User_ChangePassword Model);
        Task<IdentityResult> ConfirmEmailAsync(IdentityUserProfile User, string Token);
        Task<IdentityResult> DeleteRoleAsync(IdentityRole UserProfile);
        Task<IdentityResult> DeleteUserAsync(IdentityUserProfile UserProfile);
        Task<string> GenerateEmailConfirmationTokenAsync(IdentityUserProfile User);
        Task<string> GeneratePasswordResetTokenAsync(IdentityUserProfile User);
        Task<IdentityUserProfile> GetIdentityUserByEmailAsync(string Email);
        Task<IdentityUserProfile> GetIdentityUserByIdAsync(string Id);
        Task<IdentityUserProfile> GetIdentityUserByNameAsync(string Name);
        Task<SignInResult> LoginAsync(Auth_Login login);
        Task<IdentityResult> ResetPasswordAsync(IdentityUserProfile User, string Token, string NewPassword);
        Task<IdentityResult> UpdateUserAsync(IdentityUserProfile UpdateModel);
    }
}