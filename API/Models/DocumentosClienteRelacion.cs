using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosClienteRelacion
{
    public int IdDocumentoClienteRelacion { get; set; }

    public int? IdDocumentoCliente1 { get; set; }

    public int? IdDocumentoCliente2 { get; set; }
}
