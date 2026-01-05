using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class BancoCuentaMovimientoConciliado
{
    public int IdMovimientoConciliado { get; set; }

    public int? IdBancoCuenta { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Concepto { get; set; }

    public decimal? Monto { get; set; }

    public DateTime? FechaAlta { get; set; }

    public int? IdBancoCuentaMovimiento { get; set; }
}
