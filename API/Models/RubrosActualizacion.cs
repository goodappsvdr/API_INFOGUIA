using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class RubrosActualizacion
{
    public int IdRubroActualizacion { get; set; }

    public int? IdRubro { get; set; }

    public decimal? Alicuota { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
