using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class OrdenesExrtaccione
{
    public int IdOrdenExtraccion { get; set; }

    public int? IdComprobanteTipo { get; set; }

    public DateTime? FechaEmision { get; set; }

    public int? IdBancoCuenta { get; set; }

    public string? Letra { get; set; }

    public string? PuntoVenta { get; set; }

    public string? Numero { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdEmpresa { get; set; }

    public int? IdPlanillaCaja { get; set; }

    public decimal? Total { get; set; }

    public string? Observaciones { get; set; }

    public int? Estado { get; set; }
}
