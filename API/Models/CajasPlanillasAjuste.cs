using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class CajasPlanillasAjuste
{
    public int IdCajaPlanillaAjuste { get; set; }

    public int? IdPlanillaCaja { get; set; }

    public int? IdPlanillaCajaDetalle { get; set; }

    public int? IdTipo { get; set; }

    public int? IdUsuario { get; set; }

    public string? PuntoVenta { get; set; }

    public string? Numero { get; set; }

    public DateTime? FechaEmision { get; set; }

    public int? IdItem { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Total { get; set; }

    public int? IdEstado { get; set; }

    public int? IdSucursal { get; set; }
}
