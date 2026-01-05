using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ContabilidadCuenta
{
    public int IdContabilidadCuenta { get; set; }

    public int? IdSucursal { get; set; }

    public string? Nombre { get; set; }

    public int? IdContabilidadCuentaPadre { get; set; }

    public bool? EsCuentaGrupo { get; set; }

    public string? Tag { get; set; }

    public int? Estado { get; set; }
}
