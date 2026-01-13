using Api.Shared.DTOs.Roles;

namespace Api.Infrastructure.Services.Roles
{
    public interface IRolesServices
    {
        // CREATE
        Task<RoleDto> CreateAsync(CreateRoleDto dto);

        // READ
        Task<List<RoleDto>> GetAllAsync();
        Task<RoleDto> GetByIdAsync(int id);

        // UPDATE
        Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto);

        // DELETE
        Task DeleteAsync(int id);
    }
}