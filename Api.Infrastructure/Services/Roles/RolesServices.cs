using Api.Infrastructure.Exceptions;
using Api.Shared.DTOs.Roles;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Infrastructure.Services.Roles
{
    public class RolesServices : IRolesServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RolesServices> _logger;

        public RolesServices(
                Context  context,
            IMapper mapper,
            ILogger<RolesServices> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // ===================== CREATE =====================

        public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
        {
            // Validar si ya existe un rol con el mismo nombre
            var duplicated = await _context.Roles
                .AnyAsync(x => x.Name.ToLower() == dto.Name.ToLower());

            if (duplicated)
                throw new BadRequestException($"The role '{dto.Name}' already exists.");

            // Suponiendo que tu entidad se llama Role
            var entity = _mapper.Map<Role>(dto);

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New role created: {RoleName} with ID {Id}", entity.Name, entity.RoleId);

            return _mapper.Map<RoleDto>(entity);
        }

        // ===================== GET ALL =====================

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var roles = await _context.Roles
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();

            return _mapper.Map<List<RoleDto>>(roles);
        }

        // ===================== GET BY ID =====================

        public async Task<RoleDto> GetByIdAsync(int id)
        {
            var role = await _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoleId == id);

            if (role == null)
                throw new NotFoundException($"Role with ID {id} not found");

            return _mapper.Map<RoleDto>(role);
        }

        // ===================== UPDATE =====================

        public async Task<RoleDto> UpdateAsync(int id, UpdateRoleDto dto)
        {
            if (id != dto.RoleId)
                throw new BadRequestException("ID mismatch");

            var entity = await _context.Roles
                .FirstOrDefaultAsync(x => x.RoleId == id);

            if (entity == null)
                throw new NotFoundException($"Role with ID {id} not found");

            // Validar que el nuevo nombre no lo tenga otro rol
            var nameExists = await _context.Roles
                .AnyAsync(x => x.Name.ToLower() == dto.Name.ToLower() && x.RoleId != id);

            if (nameExists)
                throw new BadRequestException($"The role name '{dto.Name}' is already in use.");

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Role {Id} updated successfully", id);

            return _mapper.Map<RoleDto>(entity);
        }

        // ===================== DELETE =====================

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Roles
                .FirstOrDefaultAsync(x => x.RoleId == id);

            if (entity == null)
                throw new NotFoundException($"Role with ID {id} not found");

            // Nota: En tablas de sistema como Roles, a veces se prefiere validación 
            // de integridad (ej: no borrar si hay usuarios asignados) en lugar de soft delete.
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Role {Id} deleted from database", id);
        }
    }
}