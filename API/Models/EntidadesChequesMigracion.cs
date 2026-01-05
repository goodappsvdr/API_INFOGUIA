using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EntidadesChequesMigracion
{
    public string? NroCheque { get; set; }

    public string? Banco { get; set; }

    public string? BancoSucursal { get; set; }

    public DateTime? FechaCheque { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public decimal? Importe { get; set; }

    public string? Entidad { get; set; }

    public string? Cuenta { get; set; }

    public string? Titular { get; set; }
}
