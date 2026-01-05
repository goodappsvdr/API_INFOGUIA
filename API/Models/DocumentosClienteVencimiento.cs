using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosClienteVencimiento
{
    public int IdDocumentoClienteVencimiento { get; set; }

    public int? IdDocumentoCliente { get; set; }

    public DateTime? FechaVencimiento { get; set; }
}
