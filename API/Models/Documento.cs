using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Documento
{
    public int IdDocumento { get; set; }

    public int? IdDocumentoTipo { get; set; }

    public int? IdTipo { get; set; }

    public string? Nombre { get; set; }

    public string? Archivo { get; set; }

    public bool? Activo { get; set; }

    public bool? RequiereFirma { get; set; }
}
