using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class PersonalLegajoGrupoFamiliar
{
    public int IdPersonalLegajoGrupoFamiliar { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public int? TipoFamiliar { get; set; }

    public string? Apellido { get; set; }

    public string? Nombre { get; set; }

    public string? Dni { get; set; }

    public DateTime? FechaNacimiento { get; set; }
}
