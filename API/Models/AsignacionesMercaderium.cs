using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class AsignacionesMercaderium
{
    public int IdAsignacionMercaderiaDetalle { get; set; }

    public int? IdAsignacionMercaderia { get; set; }

    public int? IdDocumentoProveedorDetalle { get; set; }

    public int? IdDocumentoClienteDetalle { get; set; }

    public int? IdVendedor { get; set; }

    public int? IdCliente { get; set; }

    public decimal? NroTropa { get; set; }

    public decimal? Correlativo { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaEmision { get; set; }

    public int? IdItem { get; set; }
}
