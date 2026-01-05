using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EntidadesSucursale
{
    public int IdEntidadSucursal { get; set; }

    public int IdEntidad { get; set; }

    public int IdSucursal { get; set; }

    public int? Estado { get; set; }
}
