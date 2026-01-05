using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosProveedorObservacione
{
    public int IdDocumentoProveedorObservacion { get; set; }

    public int? IdDocumentoProveedor { get; set; }

    public string? Observaciones { get; set; }
}
