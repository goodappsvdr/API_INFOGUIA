using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class BancoChequesConciliacione
{
    public int IdBancoChequeConciliacion { get; set; }

    public int? IdBancoCuenta { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaEmision { get; set; }

    public string? Letra { get; set; }

    public string? PuntoVenta { get; set; }

    public string? Numero { get; set; }

    public int? Tipo { get; set; }

    public int? IdBanco { get; set; }

    public int? IdBancoSucursal { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdSucursal { get; set; }

    public int? IdEmpresa { get; set; }

    public int? Estado { get; set; }
}
