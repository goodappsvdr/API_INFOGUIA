using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Tarjeta
{
    public int IdTarjeta { get; set; }

    public string? Descripcion { get; set; }

    public int? IdEstado { get; set; }
}
