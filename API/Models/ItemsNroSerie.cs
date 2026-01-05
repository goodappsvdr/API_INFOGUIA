using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ItemsNroSerie
{
    public int IdItemNroSerie { get; set; }

    public int? IdItem { get; set; }

    public string? NroSerie { get; set; }

    public int? IdSucursal { get; set; }

    public int? IdEmpresa { get; set; }

    public int? Estado { get; set; }
}
