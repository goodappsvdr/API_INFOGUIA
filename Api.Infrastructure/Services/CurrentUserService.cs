using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly string? _userId;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            // Leemos el ID del usuario desde el Claim del Token JWT
            _userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string? GetUserID() => _userId;
    }
}
