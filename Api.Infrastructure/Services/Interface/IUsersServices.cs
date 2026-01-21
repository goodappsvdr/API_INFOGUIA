

using Api.Shared.DTOs.Auth;
using Api.Shared.DTOs.Listings;
using Api.Shared.DTOs.Users;

namespace Api.Infrastructure.Services.Interface
{
    public interface IUsersServices
    {
        Task<Jwt_Claims> GetClaimsAsync(string username);
        Task<List<UserDto>> GetAllUserAsync();
        Task<List<UserDto>> GetByUserIdAsync(int userId);
        Task<bool> LoginAsync(Auth_Login login);
        Task<UpdateUserDto> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
    }
}
