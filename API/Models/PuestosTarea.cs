using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class PuestosTarea
{
    public int IdPuestoTarea { get; set; }

    public int? IdPuesto { get; set; }

    public string? Descripcion { get; set; }

    public string? DescripcionResumida { get; set; }
}
