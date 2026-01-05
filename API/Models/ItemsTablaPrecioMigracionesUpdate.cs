using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ItemsTablaPrecioMigracionesUpdate
{
    public string? Descripcion { get; set; }

    public string? CodFabrica { get; set; }

    public decimal? Neto { get; set; }
}
