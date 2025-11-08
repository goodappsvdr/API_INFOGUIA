using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Shared.Interface
{
    /// <summary>
    /// Interfaz para entidades que requieren auditoría
    /// (saber quién y cuándo creó/modificó un registro)
    /// </summary>
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        string? CreatedByUserID { get; set; }
        DateTime? ModifiedAt { get; set; }
        string? ModifiedByUserID { get; set; }
    }
}
