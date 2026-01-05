using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Aviso
{
    public int IdAviso { get; set; }

    public int? IdAvisoTipo { get; set; }

    public DateTime? FechaEmision { get; set; }

    public string? DiasCorresponden { get; set; }

    public string? DiasDescanso { get; set; }

    public DateTime? FechaComienzoLicencia { get; set; }

    public DateTime? FechaHasta { get; set; }

    public DateTime? FechaReingreso { get; set; }

    public string? DiasResta { get; set; }

    public int? Jefe { get; set; }

    public int? IdPersonalLejago { get; set; }

    public DateTime? FechaNotificacion { get; set; }
}
