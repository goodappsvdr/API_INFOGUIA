using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosClienteRemito
{
    public int IdDocumentoClienteRemito { get; set; }

    public int? IdDocumentoCliente { get; set; }

    public int? IdRemito { get; set; }

    public int? IdComprobanteTipo { get; set; }
}
