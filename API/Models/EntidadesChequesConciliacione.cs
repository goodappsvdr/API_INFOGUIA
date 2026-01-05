using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EntidadesChequesConciliacione
{
    public int IdEntidadChequeConciliacion { get; set; }

    public DateTime? FechaEmision { get; set; }

    public string? Letra { get; set; }

    public string? PuntoVenta { get; set; }

    public string? Numero { get; set; }

    public int? Tipo { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdSucursal { get; set; }

    public int? IdEmpresa { get; set; }

    public int? Estado { get; set; }
}
