using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Unidade
{
    public int IdUnidad { get; set; }

    public string? Dominio { get; set; }

    public int? IdTransporte { get; set; }

    public int? IdEmpresa { get; set; }

    public int? Estado { get; set; }
}
