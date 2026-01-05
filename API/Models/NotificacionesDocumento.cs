using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class NotificacionesDocumento
{
    public int IdNotDocumentos { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public int? IdDocumento { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public DateTime? FechaVisualizado { get; set; }

    public DateTime? FechaApertura { get; set; }

    public DateTime? FechaEstado { get; set; }

    public int? EstadoFirma { get; set; }

    public bool? Firmado { get; set; }

    public string? Archivo { get; set; }
}
