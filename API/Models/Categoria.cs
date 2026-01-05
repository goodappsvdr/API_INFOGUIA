using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public string? CategoriaTipo { get; set; }

    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public int? Orden { get; set; }
}
