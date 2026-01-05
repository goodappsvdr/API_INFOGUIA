using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ContabilidadEjercicio
{
    public int IdContabilidadEjercicio { get; set; }

    public int? IdSucursal { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public int? Estado { get; set; }
}
