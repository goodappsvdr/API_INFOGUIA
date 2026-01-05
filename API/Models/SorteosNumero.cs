using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class SorteosNumero
{
    public int IdSorteoNro { get; set; }

    public int? IdSorteo { get; set; }

    public string Numero { get; set; } = null!;
}
