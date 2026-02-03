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
            Context context,
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
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("El nombre del rol es obligatorio.");

            var duplicated = await _context.Roles
                .AnyAsync(x => x.Name.ToLower() == dto.Name.ToLower());

            if (duplicated)
                throw new BadRequestException($"El rol '{dto.Name}' ya existe.");

            var entity = _mapper.Map<Role>(dto);

            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleDto>(entity);
        }


        // ===================== GET ALL =====================

        public async Task<List<RoleDto>> GetAllAsync()
        {
            var roles = await _context.Roles
                .AsNoTracking()
                .OrderByDescending(x => x.RoleId)
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
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BadRequestException("El nombre del rol es obligatorio.");

            var entity = await _context.Roles.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException($"No se encontró el rol con ID {id}.");

            var duplicated = await _context.Roles
                .AnyAsync(x => x.RoleId != id && x.Name.ToLower() == dto.Name.ToLower());

            if (duplicated)
                throw new BadRequestException($"El rol '{dto.Name}' ya existe.");

            // Mapear cambios
            _mapper.Map(dto, entity);

            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();

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