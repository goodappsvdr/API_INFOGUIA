using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class SubClasificacione
{
    public int SubClasificacion { get; set; }

    public string? Letra { get; set; }

    public string? Descripcion { get; set; }

    public string? Peso { get; set; }

    public int? IdRaza { get; set; }

    public string? Raza { get; set; }

    public int? Estado { get; set; }
}
