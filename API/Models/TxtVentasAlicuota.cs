using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class TxtVentasAlicuota
{
    public int TxtVentaAlicuota { get; set; }

    public string? TipoComprobante { get; set; }

    public string? PuntoVenta { get; set; }

    public string? NroComprobante { get; set; }

    public decimal? ImporteNeto { get; set; }

    public string? Alicuota { get; set; }

    public decimal? ImporteLiquidado { get; set; }

    public int? Mes { get; set; }

    public int? Anio { get; set; }

    public DateTime? FechaAlta { get; set; }

    public int? IdComprobante { get; set; }

    public int? IdComprobanteTipo { get; set; }
}
