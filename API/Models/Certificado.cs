using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Certificado
{
    public int IdCertificadoMedico { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public string? Tipo { get; set; }

    public string? Imagen { get; set; }

    public string? Descripcion { get; set; }

    public string? CantidadDias { get; set; }

    public DateTime? Fecha { get; set; }
}
