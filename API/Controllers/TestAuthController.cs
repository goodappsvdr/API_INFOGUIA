using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase
    {
        // ✅ Este endpoint requiere autenticación
        [HttpGet("privado")]
        [Authorize]
        public IActionResult GetPrivado()
        {
            var userName = User.Identity?.Name ?? "Usuario sin nombre";
            return Ok(new { mensaje = $"Hola {userName}, accediste a un endpoint protegido." });
        }

        // 🚪 Este endpoint es público (sin token)
        [HttpGet("publico")]
        [AllowAnonymous]
        public IActionResult GetPublico()
        {
            return Ok(new { mensaje = "Este endpoint es público, no necesita token." });
        }
    }
}
