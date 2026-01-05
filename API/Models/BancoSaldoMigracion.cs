using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class BancoSaldoMigracion
{
    public int? IdCuenta { get; set; }

    public DateTime? Fecha { get; set; }

    public string? NroComprobante { get; set; }

    public string? Concepto { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Monto { get; set; }

    public decimal? SaldoParcial { get; set; }
}
