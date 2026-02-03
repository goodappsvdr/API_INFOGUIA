
using Api.Shared.Models;


using Api.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;
using Api.Shared.DTOs.Categories;
using Api.Shared.DTOs.Users;
using Microsoft.Extensions.Logging;
using Api.Infrastructure.Services.Categories;
using Api.Infrastructure.Exceptions;
using Api.Shared.DTOs.Listings;

namespace Api.Infrastructure.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly Context _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersServices> _logger;

        public UsersServices(
            Context context,
            IMapper mapper,
            ILogger<UsersServices> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<bool> LoginAsync(Auth_Login login)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Email.ToLower() == login.Username.ToLower() &&
                    x.IsActive == true);

            if (user == null)
                return false;

            return user.PasswordHash == login.Password;
        }

        public async Task<Jwt_Claims> GetClaimsAsync(string username)
        {
            var data = await (
                from u in _context.Users.AsNoTracking()
                join r in _context.Roles.AsNoTracking()
                    on u.RoleId equals r.RoleId
                where u.Email == username && u.IsActive
                select new
                {
                    User = u,
                    RoleName = r.Name,
                    RoleId = r.RoleId,
                     ImgProfile = u.ImgProfile
                }
            ).FirstOrDefaultAsync();

            if (data == null)
                return null;

            Jwt_Claims claim = new Jwt_Claims
            {
                UserId = data.User.UserId ,
                Email = data.User.Email,
                FirstName = data.User.FirstName,
                LastName = data.User.LastName,
                TenantId = data.User.TenantId,
                RoleId = data.RoleId,
                RoleName = data.RoleName,
                Status = data.User.IsActive ? "Activo" : "Inactivo",
                ImgProfile = data.ImgProfile

            };

            claim.BranchOffices = await (
                from us in _context.BranchsUsers
                join bo in _context.BranchsOffices
                    on us.BranchOfficeId equals bo.BranchOfficeId
                where us.UserId == data.User.UserId
                select new Jwt_Claims_BracnhOffice
                {
                    BranchId = bo.BranchOfficeId,


                    Description = bo.Name,
                    PointSale = bo.SalesPoint
                }
            ).ToListAsync();

            return claim;
        }
        public async Task<List<GetUsers>> GetAllUserAsync()
        {
            try
            {
                var users = await (
                    from u in _context.Users.AsNoTracking()
                    join r in _context.Roles.AsNoTracking()
                        on u.RoleId equals r.RoleId
                    select new GetUsers
                    {
                        UserId = u.UserId,
                        TenantId = u.TenantId,
                        RoleId = u.RoleId,

                        Email = u.Email,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        ImgProfile = u.ImgProfile,

                        IsActive = u.IsActive,

                        CreatedAt = u.CreatedAt,
                        CreatedByUserId = u.CreatedByUserId ?? 0,
                        ModifiedAt = u.ModifiedAt ?? DateTime.Now,
                        ModifiedByUserId = u.ModifiedByUserId ?? 0,
                        Rol = r.Name
                    }
                ).OrderByDescending(x => x.UserId)
                .ToListAsync();

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }



        /// <summary>
        /// Buscar usuario por ID
        /// </summary>
        public async Task<List<UserDto>> GetByUserIdAsync(int userId)
        {
            try
            {
                // Usamos .Where() para obtener una colección filtrada
                var users = await _context.Users
                    .AsNoTracking()
                    .Where(u => u.UserId == userId)
                    .ToListAsync();

                // El mapper transformará List<User> a List<UserDto> automáticamente
                return _mapper.Map<List<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<UpdateUserDto> UpdateUserAsync(int userID, UpdateUserDto updateUserDto)
        {
            try
            {
                // 1. Buscar el usuario incluyendo las validaciones necesarias
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.UserId == userID);

                if (user == null)
                {
                    throw new NotFoundException($"User with ID {userID} not found");
                }

                // 2. Mapeo de datos: Volcamos lo que viene del DTO al objeto 'user' rastreado por EF
                _mapper.Map(updateUserDto, user);

                // 3. Auditoría: Forzamos los valores de modificación
                user.ModifiedAt = DateTime.UtcNow;

                // Convertimos a string si tu entidad espera un GUID o String, 
                // de lo contrario, deja solo userID si es int en la DB.
                user.ModifiedByUserId = userID;

                // 4. Guardar cambios
                await _context.SaveChangesAsync();

                _logger.LogInformation("User {UserId} updated successfully", userID);

                // 5. Devolver el DTO actualizado (Mapeo inverso)
                return _mapper.Map<UpdateUserDto>(user);
            }
            catch (NotFoundException)
            {
                // Re-lanzamos para que el controlador lo capture
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fatal al actualizar el usuario {UserId}", userID);
                throw; // Importante no perder el stack trace
            }
        }

    }
}
