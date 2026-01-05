using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EntidadesChequesConciliacioneDetalle
{
    public int IdEntidadChequeConciliacionDetalle { get; set; }

    public int? IdEntidadChequeConciliacion { get; set; }

    public string? RazonSocial { get; set; }

    public string? Banco { get; set; }

    public string? BancoSucursal { get; set; }

    public DateTime? FechaEmision { get; set; }

    public DateTime? FechaVto { get; set; }

    public string? NroCheque { get; set; }

    public decimal? Importe { get; set; }

    public string? Cuenta { get; set; }

    public string? Titular { get; set; }
}
