using Api.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.DTOs.Tenant
{
    /// <summary>
    /// Clase base para entidades con auditoría
    /// </summary>
    public abstract class BaseEntity : IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }
        public string? CreatedByUserID { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedByUserID { get; set; }
    }
}
