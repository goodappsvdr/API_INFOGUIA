using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class MigracionEntidade
{
    public string? NroCuenta { get; set; }

    public string? Fantasia { get; set; }

    public string? RazonSocial { get; set; }

    public string? Cuit { get; set; }

    public string? CategoriaIva { get; set; }

    public string? Domicilio { get; set; }

    public string? Zona { get; set; }

    public string? Observaciones { get; set; }

    public int? IdZona { get; set; }
}
