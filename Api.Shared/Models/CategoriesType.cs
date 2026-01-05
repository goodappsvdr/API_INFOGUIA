using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class CategoriesType
{
    public int IdCategoriaTipo { get; set; }

    public int? IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public bool? Activo { get; set; }
}
