using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Raza
{
    public int IdRaza { get; set; }

    public string? Descripcion { get; set; }

    public string? Observaciones { get; set; }

    public int? Estado { get; set; }

    public int? Tipo { get; set; }
}
