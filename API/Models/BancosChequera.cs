using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class BancosChequera
{
    public int IdBancosChequeras { get; set; }

    public int? IdBancoCuenta { get; set; }

    public decimal? Cantidad { get; set; }

    public long? Desde { get; set; }

    public long? Hasta { get; set; }

    public int? Tipo { get; set; }

    public int? Estado { get; set; }
}
