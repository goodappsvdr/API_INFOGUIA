using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Noticia
{
    public int IdNoticia { get; set; }

    public string? Imagen { get; set; }

    public string? Titulo { get; set; }

    public string? Contenido { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public bool? Activo { get; set; }
}
