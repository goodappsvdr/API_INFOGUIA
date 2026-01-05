using Api.Infrastructure.Jwt;
using Api.Infrastructure.Services.Interface;
using Api.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador de Usuarios
    /// </summary>
    [ApiController, AllowAnonymous, Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersServices _usersServices;
        private readonly Jwt_AccessTokenSettings _accessTokenSettings;
        private readonly Jwt_RefreshTokenSettings _refreshTokenSettings;

        /// <summary>
        /// Constructor para inicializar el controlador de Usuarios
        /// </summary>
        /// <param name="usersServices">La interfaz para operaciones de Usuario</param>
        /// <param name="accessTokenSettings">Clase de configuracion del access token</param>
        /// <param name="refreshTokenSettings">Clase de configuracion del refresh token</param>
        public UsersController(IUsersServices usersServices, Jwt_AccessTokenSettings accessTokenSettings, Jwt_RefreshTokenSettings refreshTokenSettings)
        {
            _usersServices = usersServices;
            _accessTokenSettings = accessTokenSettings;
            _refreshTokenSettings = refreshTokenSettings;
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
    }
}
