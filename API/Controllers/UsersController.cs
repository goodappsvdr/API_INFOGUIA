using Api.Shared.Data;
using Api.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔐 Solo con token válido de Auth0
    public class UsersController : ControllerBase
    {
        private readonly Context _context;

        public UsersController(Context context)
        {
            _context = context;
        }

        // GET api/users/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var auth0Id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(auth0Id))
                return Unauthorized("No se encontró el ID de Auth0 en el token.");

            // 🔎 Buscar usuario por Email o Auth0Id (si lo tenés almacenado)
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var user = await _context.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                // Si no existe, podés crearlo o devolver error
                return NotFound("Usuario no encontrado en la base de datos local.");
            }

            return Ok(new
            {
                user.UserId,
                user.Email,
                Nombre = $"{user.FirstName} {user.LastName}",
                user.RoleId,
                user.ImgProfile
            });
        }
    }
}
