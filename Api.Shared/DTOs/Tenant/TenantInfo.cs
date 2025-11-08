using Finbuckle.MultiTenant.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
namespace Api.Shared.DTOs.Tenant
{
    public class TenantInfo : ITenantInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        // Propiedades adicionales personalizadas
        public string? DatabaseName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
