using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Token
{
    public int IdToken { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public string? Token1 { get; set; }

    public bool? Activo { get; set; }
}
