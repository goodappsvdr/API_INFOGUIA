using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class MigrarLocalidade
{
    public string? IdLocalidad { get; set; }

    public string? Localidad { get; set; }

    public string? IdProvincia { get; set; }

    public string? Provincia { get; set; }
}
