using Api.Shared.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Api.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserID()
        {
            // Obtener el usuario del contexto HTTP
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                return null;

            // Intenta obtener el ID del usuario de los claims
            // Puede estar en diferentes claims dependiendo de tu configuración JWT
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? user.FindFirst("sub")?.Value
                      ?? user.FindFirst("userId")?.Value
                      ?? user.FindFirst("id")?.Value;

            return userId;
        }
    }
}