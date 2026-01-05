using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Sancione
{
    public int IdSancion { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public int? LegajoSolicitante { get; set; }

    public int? LegajoSancionado { get; set; }

    public string? Observacion { get; set; }

    public int? TipoSancion { get; set; }

    public string? DiasSuspendido { get; set; }

    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }

    public DateTime? Reincorporarse { get; set; }
}
