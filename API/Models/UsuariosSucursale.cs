using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class UsuariosSucursale
{
    public int IdUsuarioSucursal { get; set; }

    public int? IdUsuario { get; set; }

    public int? IdSucursal { get; set; }
}
