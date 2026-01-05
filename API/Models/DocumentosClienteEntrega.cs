using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosClienteEntrega
{
    public int IdDocumentoClienteEntrega { get; set; }

    public string? Descripcion { get; set; }

    public string? Direccion { get; set; }

    public int? IdProvincia { get; set; }

    public long? IdLocalidad { get; set; }

    public int? IdDocumentoCliente { get; set; }

    public int? IdComprobanteTipo { get; set; }
}
