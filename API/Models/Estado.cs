using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? Categoria { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? Imagen { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Paise> Paises { get; set; } = new List<Paise>();
}
