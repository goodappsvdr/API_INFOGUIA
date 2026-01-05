using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ElementosCobrosDolare
{
    public int IdElementoCobroDolar { get; set; }

    public int? IdComprobanteTipo { get; set; }

    public int? IdComprobante { get; set; }

    public decimal? MontoDolar { get; set; }

    public decimal? Cotizacion { get; set; }

    public decimal? MontoPeso { get; set; }

    public int? IdCajaPlanilla { get; set; }

    public int? IdCajaPlanillaDetalle { get; set; }
}
