using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class NotificacionesNoticia
{
    public int IdNotNoticia { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public int? IdNoticia { get; set; }

    public bool? Estado { get; set; }
}
