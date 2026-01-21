using Api.Infrastructure.Jwt;
using Api.Infrastructure.Services.Interface;
using Api.Shared.Data;
using Api.Shared.DTOs.Auth;
using Api.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Google.Apis.Auth;
using Api.Shared.DTOs.Users;
using API.Extensions;
using Api.Infrastructure.Exceptions;
using Api.Shared.DTOs.Listings;
using Api.Shared.DTOs;
using API.Controllers;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador de Usuarios
    /// </summary>
    [ApiController, AllowAnonymous, Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ContextInfoGuia _context;
        private readonly IUsersServices _usersServices;
        private readonly Jwt_AccessTokenSettings _accessTokenSettings;
        private readonly Jwt_RefreshTokenSettings _refreshTokenSettings;
        private readonly ILogger<ListingsController> _logger;

        /// <summary>
        /// Constructor para inicializar el controlador de Usuarios
        /// </summary>
        /// <param name="usersServices">La interfaz para operaciones de Usuario</param>
        /// <param name="accessTokenSettings">Clase de configuracion del access token</param>
        /// <param name="refreshTokenSettings">Clase de configuracion del refresh token</param>
        public UsersController(
        IUsersServices usersServices,
        Jwt_AccessTokenSettings accessTokenSettings,
        Jwt_RefreshTokenSettings refreshTokenSettings,
        ContextInfoGuia context,
         ILogger<ListingsController> logger)

        {
            _usersServices = usersServices;
            _accessTokenSettings = accessTokenSettings;
            _refreshTokenSettings = refreshTokenSettings;
            _context = context;
         _logger = logger;
        }


        /// <summary>
        /// Obtiene las claims del usuarios atravez del token.
        /// </summary>
        /// <returns>las claims del token del usuario, NoContent si no se encuentra ningun dato.</returns>
        [Authorize, HttpGet("GetUserClaims")]
        public async Task<ActionResult> GetUserClaims()
        {
            Jwt_Claims Claims = Jwt_Helpers.GetClaimsByToken(Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));

            try
            {
                var Userclaims = await _usersServices.GetClaimsAsync(Claims.Email);

                if (Userclaims == null) return NoContent();

                return Ok(Userclaims);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] Auth_Login model)
        {
            try
            {
                var loginOk = await _usersServices.LoginAsync(model);

                if (!loginOk)
                    return Unauthorized(new { Message = "El usuario o contraseña no coinciden." });

                var userClaims = await _usersServices.GetClaimsAsync(model.Username);

                if (userClaims == null)
                    return Unauthorized(new { Message = "Error al obtener claims." });

                string refreshToken = Jwt_Helpers.GetRefreshTokens(
                    userClaims,
                    _refreshTokenSettings,
                    HttpContext
                );

                Jwt_Tokens tokens = Jwt_Helpers.GetAccessTokens(
                    new Jwt_Tokens(),
                    userClaims,
                    _accessTokenSettings
                );

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        Error = "Error en Login",
                        Detalle = ex.Message,
                        Causa = ex.InnerException?.Message
                    }
                );
            }
        }


        /// <summary>
        /// Obtiene un nuevo AccessToken pasando el refreshToken.
        /// </summary>
        /// <returns>Una respuesta exitosa con tokens de acceso, BadRequest si las RefreshToken es invalido o vencido o un error interno del servidor.</returns>
        [HttpPost("Refresh")]
        public async Task<ActionResult> GetAccessTokenByRefreshToken()
        {
            Jwt_Claims Claims = Jwt_Helpers.GetClaimsByToken(Request.Cookies["RefreshToken"].ToString().Replace("Bearer ", ""));
            try
            {
                string DateExpirationToken = Jwt_Helpers.GetDateExpirationByToken(Request.Cookies["RefreshToken"].ToString().Replace("Bearer ", ""));

                DateTimeOffset DateExpiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(DateExpirationToken)).UtcDateTime;

                if (DateExpiration < DateTimeOffset.Now) return Unauthorized(new { Message = "El RefreshToken esta vencido." });

                var UserClaims = await _usersServices.GetClaimsAsync(Claims.Username);

                if (UserClaims == null) return Unauthorized(new { Message = "No se encontro el usuario del cual pertenece el RefreshToken." });

                Jwt_Tokens Tokens = Jwt_Helpers.GetAccessTokens(new Jwt_Tokens(), UserClaims, _accessTokenSettings);

                return Ok(Tokens);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Crear un nuevo Usuario
        /// </summary>
        /// 
        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUserAsync(createUser_Input input)
        {
            try
            {
                var usermodel = new User
                {
                    TenantId = 1,
                    RoleId = 1,
                    Email = input.email ?? string.Empty,
                    PasswordHash = input.password ?? string.Empty,
                    FirstName = "",
                    LastName = "",
                    ImgProfile = "",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = null,
                    ModifiedAt = DateTime.UtcNow,
                    ModifiedByUserId = null
                };

                // 1️⃣ Insert → genera UserId
                _context.Users.Add(usermodel);
                await _context.SaveChangesAsync();

                // 2️⃣ Usar el ID autogenerado
                usermodel.CreatedByUserId = usermodel.UserId;
                usermodel.ModifiedByUserId = usermodel.UserId;

                _context.Users.Update(usermodel);
                await _context.SaveChangesAsync();

                return Ok(usermodel.UserId);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = "Error creando usuario",
                    Detalle = ex.Message,
                    Inner = ex.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Crear un nuevo Usuario desde la cuenta de Gmail
        /// </summary>
        /// 
        [HttpPost("LoginWithGoogle")]
        public async Task<ActionResult> LoginWithGoogle([FromBody] GoogleLoginInput input)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[]
                    {
                "198608702380-0p9in3tfc6n2qsucfs4guj3ccu318qv4.apps.googleusercontent.com"
            }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    input.IdToken,
                    settings
                );

                string email = payload.Email;

                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (user == null)
                {
                    user = new User
                    {
                        TenantId = 1,
                        RoleId = 1,
                        Email = email,
                        PasswordHash = "", // 🔐 NO GOOGLE
                        FirstName = payload.GivenName ?? "",
                        LastName = payload.FamilyName ?? "",
                        ImgProfile = payload.Picture,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        CreatedByUserId = 0,
                        ModifiedAt = DateTime.UtcNow,
                        ModifiedByUserId = 0
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    user.CreatedByUserId = user.UserId;
                    user.ModifiedByUserId = user.UserId;
                    await _context.SaveChangesAsync();
                }

                if (!user.IsActive)
                    return Unauthorized("Usuario inactivo");

                var claims = await _usersServices.GetClaimsAsync(user.Email);

                var tokens = Jwt_Helpers.GetAccessTokens(
                    new Jwt_Tokens(),
                    claims,
                    _accessTokenSettings
                );

                return Ok(tokens);
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    Error = "Token Google inválido",
                    Detalle = ex.Message
                });
            }
        }

        /// <summary>
        /// Crear un nuevo Usuario Desde Super Admin
        /// </summary>
        /// 
        [HttpPost("CreateUserSuperAdmin")]
        public async Task<ActionResult> CreateUserSuperAdminAsync(createUserSuperAdmin_Input input)
        {
            try
            {
                var usermodel = new User
                {
                    TenantId = 1,
                    RoleId = input.roleId ?? 1,
                    Email = input.email ?? string.Empty,
                    PasswordHash = input.password ?? string.Empty,
                    FirstName = input.firstName,
                    LastName = "",
                    ImgProfile = "",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = input.userId,
                    ModifiedAt = DateTime.UtcNow,
                    ModifiedByUserId = input.userId
                };

                _context.Users.Add(usermodel);
                await _context.SaveChangesAsync();

                return Ok(usermodel.UserId);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = "Error creando usuario",
                    Detalle = ex.Message,
                    Inner = ex.InnerException?.Message
                });
            }
        }


        /// <summary>
        /// Obtiene todos los Usuarios
        /// </summary>
        [Authorize, HttpGet("GetAllUser")]
        public async Task<ActionResult> GetAllUserAsync()
        {
           
            try
            {
                var user = await _usersServices.GetAllUserAsync();

                if (user == null) return NoContent();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Obtiene un usuario por ID
        /// </summary>
        [Authorize, HttpGet("GetByUserId")]
        public async Task<ActionResult> GetByUserIdAsync(int userId)
        {

            try
            {
                var user = await _usersServices.GetByUserIdAsync(userId);

                if (user == null) return NoContent();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        /// <summary>
        /// Actualiza la información de un usuario
        /// </summary>
        /// <param name="id">ID del usuario a actualizar (desde la URL)</param>
        /// <param name="updateUserDto">Objeto con los nuevos datos</param>
        [Authorize]
        [HttpPut("UpdateUser/{id}")]
        [ProducesResponseType(typeof(ApiResponse<UpdateUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
              

                // 2. Llamar al servicio
                var updatedUser = await _usersServices.UpdateUserAsync(id, updateUserDto);

                return Ok(ApiResponse<UpdateUserDto>.SuccessResponse(updatedUser, "Usuario actualizado correctamente."));
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Usuario no encontrado: {UserId}", id);
                return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning(ex, "No autorizado para actualizar usuario: {UserId}", id);
                return StatusCode(403, ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, "Petición incorrecta al actualizar usuario: {UserId}", id);
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al actualizar usuario {UserId}", id);
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Ocurrió un error interno al actualizar el usuario."));
            }
        }
    }
}
