using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class BancosChequesMigracion
{
    public int? IdBancoCuenta { get; set; }

    public string? RazonSocial { get; set; }

    public string? NroCheque { get; set; }

    public DateTime? Emision { get; set; }

    public DateTime? Vencimiento { get; set; }

    public decimal? Importe { get; set; }

    public int? IdBancoChequera { get; set; }
}
