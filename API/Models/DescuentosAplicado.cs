using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DescuentosAplicado
{
    public int IdDescuento { get; set; }

    public int IdDescuentoTipo { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdComprobante { get; set; }

    public int IdComprobanteTipo { get; set; }

    public string? NroComprobante { get; set; }

    public DateTime? FechaEmision { get; set; }

    public decimal Total { get; set; }

    public int Estado { get; set; }

    public int IdEntidad { get; set; }

    public int IdUsuario { get; set; }
}
