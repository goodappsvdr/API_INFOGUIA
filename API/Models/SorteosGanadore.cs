using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class SorteosGanadore
{
    public int IdSorteoGanador { get; set; }

    public int? IdSorteo { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Numero { get; set; }

    public int? Posicion { get; set; }
}
