using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Curso
{
    public int IdCurso { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public int? IdDocumentosTipo { get; set; }

    public string? Descripcion { get; set; }

    public string? Archivo { get; set; }

    public DateTime? Fecha { get; set; }
}
