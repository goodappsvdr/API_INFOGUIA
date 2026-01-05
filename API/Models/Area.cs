using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Area
{
    public int IdArea { get; set; }

    public string? Nombre { get; set; }

    public int? IdSeccionPadre { get; set; }

    public int? IdJefe { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public string? SeccionPadre { get; set; }

    public int? Orden { get; set; }
}
