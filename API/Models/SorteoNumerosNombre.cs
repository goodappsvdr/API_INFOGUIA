using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class SorteoNumerosNombre
{
    public int IdSorteoNro { get; set; }

    public int? IdSorteo { get; set; }

    public string? Dni { get; set; }

    public string? Nombre { get; set; }
}
