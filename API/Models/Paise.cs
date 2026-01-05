using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Paise
{
    public int IdPais { get; set; }

    public string? Descripcion { get; set; }

    public int? Orden { get; set; }

    public int? Estado { get; set; }

    public virtual Estado? EstadoNavigation { get; set; }
}
