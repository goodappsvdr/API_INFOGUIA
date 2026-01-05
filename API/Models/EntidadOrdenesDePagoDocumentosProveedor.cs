using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EntidadOrdenesDePagoDocumentosProveedor
{
    public int IdEntidadOrdenDePagoDocumentoProveedor { get; set; }

    public int? IdEntidad { get; set; }

    public int? IdOrdenDePago { get; set; }

    public string? NumeroOrdenDePago { get; set; }

    public decimal? ImporteOrdenDePago { get; set; }

    public int? IdDocumentoProveedor { get; set; }

    public int? IdComprobanteTipo { get; set; }

    public string? NumeroComprobante { get; set; }

    public decimal? ImporteComprobante { get; set; }
}
