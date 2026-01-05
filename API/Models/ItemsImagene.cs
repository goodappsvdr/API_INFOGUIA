using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ItemsImagene
{
    public int IdItemImagen { get; set; }

    public int? IdItem { get; set; }

    public string? CodFabrica { get; set; }

    public string? Imagen { get; set; }

    public int? Orden { get; set; }

    public int? Estado { get; set; }
}
