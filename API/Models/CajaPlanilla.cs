using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class CajaPlanilla
{
    public int IdPlanillaCaja { get; set; }

    public string? PuntoVenta { get; set; }

    public int? IdEmpresa { get; set; }

    public DateTime? FechaApertura { get; set; }

    public DateTime? FechaCierre { get; set; }

    public int? IdUsuario { get; set; }

    public decimal? SaldoInicial { get; set; }

    public decimal? TotalIngresos { get; set; }

    public decimal? TotalEgresos { get; set; }

    public decimal? TotalRendido { get; set; }

    public decimal? Diferencia { get; set; }

    public int? Estado { get; set; }

    public int? IdSucursal { get; set; }
}
