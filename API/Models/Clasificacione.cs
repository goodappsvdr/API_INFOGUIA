using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Clasificacione
{
    public int IdClasificacion { get; set; }

    public string? Descripcion { get; set; }

    public string? Codigo { get; set; }

    public string? Observaciones { get; set; }

    public DateTime? FechaAlta { get; set; }

    public int? IdUsuario { get; set; }

    public int? Estado { get; set; }
}
