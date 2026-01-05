using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EvaluacionObjetivo
{
    public int IdEvaluacionObjetivo { get; set; }

    public int? IdEvaluacionDesempeño { get; set; }

    public string? Objetivo { get; set; }

    public int? IdCriterio { get; set; }

    public bool? GeneroRrhh { get; set; }
}
