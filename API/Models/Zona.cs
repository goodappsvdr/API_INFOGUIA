using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Zona
{
    public int IdZona { get; set; }

    public string? Descripcion { get; set; }

    public string? Observaciones { get; set; }

    public int? Estado { get; set; }
}
