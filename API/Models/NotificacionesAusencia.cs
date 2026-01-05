using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class NotificacionesAusencia
{
    public int IdNotAusencia { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public int? IdCertificado { get; set; }

    public bool? Estado { get; set; }
}
