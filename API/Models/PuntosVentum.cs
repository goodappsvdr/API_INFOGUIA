using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class PuntosVentum
{
    public int IdPuntoVenta { get; set; }

    public string? Descripcion { get; set; }

    public int? IdComprobanteTipo { get; set; }

    public string? Letra { get; set; }

    public long? Nro { get; set; }

    public int? IdEmpresa { get; set; }

    public int? Estado { get; set; }

    public int? IdUsuario { get; set; }
}
