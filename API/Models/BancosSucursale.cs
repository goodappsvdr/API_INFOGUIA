using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class BancosSucursale
{
    public int IdBancoSucursal { get; set; }

    public int? IdBanco { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEstado { get; set; }
}
