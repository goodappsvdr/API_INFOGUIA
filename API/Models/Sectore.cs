using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Sectore
{
    public int IdSector { get; set; }

    public string? Nombre { get; set; }

    public int? IdSeccion { get; set; }

    public bool? Activo { get; set; }
}
