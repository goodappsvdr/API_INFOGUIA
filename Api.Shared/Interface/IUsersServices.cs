using Api.Shared.DTOs.Identity.AspNetUsers;
using Api.Shared.Identity;
using Api.Shared.Jwt;

namespace Api.Shared.Interface
{
    public interface IUsersServices
    {
        Task<bool> IsActive(string IdUser);
        Task<bool> IsDeleted(string IdUser);
        Task<bool> IsVerified(string IdUser);
     
        Task<IdentityUserProfile> GetIdentityUserByNameAsync(string Name);
        Task<IdentityUserProfile> GetIdentityUserByIdAsync(string Id);
        Task<IdentityUserProfile> GetIdentityUserByEmailAsync(string Email);
        Task<Jwt_Claims> GetClaimsAsync(string Username);
        Task<IdentityResult> AddAsync(User_Create CreateModel);

        Task<string> GenerateValidateTokenByEmail(string Email);
        Task<string> GenerateChangePasswordTokenByEmail(string Email);


        Task<IdentityUserProfile> ValidateEmailByToken(string Token);
        Task<IdentityUserProfile> ChangePasswordByToken(string Password, string Token);
        Task<IdentityResult> UpdateImage(IdentityUserProfile UserProfile, IFormFile Image);
        Task<IdentityResult> UpdateAsync(User_Update UpdateModel, Jwt_Claims Claims);
        Task<IdentityResult> ChangePasswordAsync(IdentityUserProfile User, User_ChangePassword Model);
        Task<IdentityResult> DeleteAsync(IdentityUserProfile UserProfile);
        Task<IdentityResult> UserDeleteAsync(IdentityUserProfile UserProfile);
    }
}
