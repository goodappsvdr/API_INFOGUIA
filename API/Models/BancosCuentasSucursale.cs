using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class BancosCuentasSucursale
{
    public int IdBancoCuentaSucursal { get; set; }

    public int? IdBancoCuenta { get; set; }

    public int? IdSucursal { get; set; }
}
