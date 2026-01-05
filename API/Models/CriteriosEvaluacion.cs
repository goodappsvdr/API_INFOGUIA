using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class CriteriosEvaluacion
{
    public int IdCriterio { get; set; }

    public string? Criterio { get; set; }

    public string? CriterioResumido { get; set; }

    public string? Detalle { get; set; }
}
