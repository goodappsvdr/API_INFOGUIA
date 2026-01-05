using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Localidade
{
    public long IdLocalidad { get; set; }

    public string? Descripcion { get; set; }

    public int? IdProvincia { get; set; }

    public string? Cp { get; set; }

    public int? Orden { get; set; }

    public int? Estado { get; set; }
}
