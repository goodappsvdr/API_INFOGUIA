using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Banco
{
    public int IdBanco { get; set; }

    public string? RazonSocial { get; set; }

    public int? CodigoEntidad { get; set; }

    public int? IdEstado { get; set; }
}
