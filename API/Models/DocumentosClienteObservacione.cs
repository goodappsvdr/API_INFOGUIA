using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosClienteObservacione
{
    public int IdDocumentoClienteObservacion { get; set; }

    public int? IdDocumentoCliente { get; set; }

    public string? Observaciones { get; set; }
}
