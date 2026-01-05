using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Descuento
{
    public int IdDescuento { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdEstado { get; set; }

    public int IdSucursal { get; set; }
}
