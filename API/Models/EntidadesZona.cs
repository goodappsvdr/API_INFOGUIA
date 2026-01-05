using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EntidadesZona
{
    public int IdEntidadZona { get; set; }

    public int? IdZona { get; set; }

    public string? Descripcion { get; set; }

    public int? IdUsuario { get; set; }

    public int? Estado { get; set; }
}
