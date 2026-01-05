using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EntidadesCategoria
{
    public int IdEntidadCategoria { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEmpresa { get; set; }

    public int? Estado { get; set; }
}
