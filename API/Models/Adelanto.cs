using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Adelanto
{
    public int IdAdelanto { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public DateTime? FechaPedido { get; set; }

    public DateTime? FechaDescuento { get; set; }

    public decimal? Importe { get; set; }

    public int? Motivo { get; set; }

    public string? Observaciones { get; set; }

    public int? Estado { get; set; }

    public int? IdJefe { get; set; }
}
