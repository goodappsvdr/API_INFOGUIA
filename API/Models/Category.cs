using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Category
{
    public int IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public string? Observacion { get; set; }

    public bool? Activo { get; set; }
}
