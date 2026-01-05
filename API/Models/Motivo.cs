using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Motivo
{
    public int IdMotivo { get; set; }

    public string? Descripcion { get; set; }

    public string? IdEmpresa { get; set; }

    public int? Estado { get; set; }
}
