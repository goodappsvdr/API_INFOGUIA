using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class BancoCheuqesConciliacioneDetalle
{
    public int IdBancoChequeConciliacionDetalle { get; set; }

    public int? IdBancoChequeConciliacion { get; set; }

    public int? IdBancoCheque { get; set; }

    public string? RazonSocial { get; set; }

    public DateTime? FechaEmision { get; set; }

    public DateTime? FechaVto { get; set; }

    public string? NroCheque { get; set; }

    public decimal? Importe { get; set; }
}
