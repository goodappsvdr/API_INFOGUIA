using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class PersonalRotacione
{
    public int IdPersonalRotacion { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public int? IdSeccionAnterior { get; set; }

    public int? IdSeccionNueva { get; set; }

    public int? IdSectorNuevo { get; set; }

    public int? IdPuestoNuevo { get; set; }

    public int? IdJefeNuevo { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public DateTime? FechaEgreso { get; set; }

    public int? IdMotivo { get; set; }

    public string? Observaciones { get; set; }
}
