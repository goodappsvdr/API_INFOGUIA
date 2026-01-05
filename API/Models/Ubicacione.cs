using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Ubicacione
{
    public int IdUbicacion { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEmpresa { get; set; }

    public int? Estado { get; set; }
}
