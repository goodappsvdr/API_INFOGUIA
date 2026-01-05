using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ElementosCobrosVario
{
    public int IdElementoCobroVario { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdEstado { get; set; }

    public int IdSucursal { get; set; }
}
