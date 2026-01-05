using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class SubRubro
{
    public int IdSubRubro { get; set; }

    public string? Descripcion { get; set; }

    public int? IdRubro { get; set; }

    public int? IdEmpresa { get; set; }

    public int? Estado { get; set; }
}
