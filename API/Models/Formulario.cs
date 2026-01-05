using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Formulario
{
    public int IdFormulario { get; set; }

    public string? Formulario1 { get; set; }

    public string? Nombre { get; set; }

    public int? IdEstado { get; set; }

    public int? Orden { get; set; }
}
