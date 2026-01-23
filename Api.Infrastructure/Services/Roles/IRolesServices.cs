using Api.Shared.DTOs.Roles;

namespace Api.Infrastructure.Services.Roles
{
    public interface IRolesServices
    {
        // CREATE
        Task<RoleDto> CreateAsync(CreateRoleDto dto);

        // UPDATE
        Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto);

        // READ
        Task<List<RoleDto>> GetAllAsync();

        // CAMBIO AQUÍ: De Task<List<RoleDto>> a Task<RoleDto>
        Task<RoleDto> GetByIdAsync(int roleId);

        // DELETE
        Task DeleteAsync(int id);
    }
}