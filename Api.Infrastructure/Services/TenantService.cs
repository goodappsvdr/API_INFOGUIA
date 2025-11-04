
using Api.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private readonly int? _tenantId;

        public TenantService(IHttpContextAccessor httpContextAccessor)
        {
            // Leemos el claim "TenantID" que pusimos en el JWT (Paso 2)
            var tenantClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue("TenantID");

            if (!string.IsNullOrEmpty(tenantClaim) && int.TryParse(tenantClaim, out int tenantId))
            {
                // Encontramos un TenantID, lo guardamos
                _tenantId = tenantId;
            }
            else
            {
                // Es SuperAdmin (NULL) o un usuario sin tenant
                _tenantId = null;
            }
        }

        public int? GetTenantID() => _tenantId;
    }
}
