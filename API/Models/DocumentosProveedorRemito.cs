using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosProveedorRemito
{
    public int IdDocumentoProveedorRemito { get; set; }

    public int? IdDocumentoProveedor { get; set; }

    public int? IdRemito { get; set; }

    public int? IdComprobanteTipo { get; set; }
}
