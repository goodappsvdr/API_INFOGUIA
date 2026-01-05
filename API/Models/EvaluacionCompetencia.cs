using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EvaluacionCompetencia
{
    public int IdEvaluacionCompetencia { get; set; }

    public int? IdEvaluacionDesempeño { get; set; }

    public int? IdCompetencia { get; set; }

    public int? IdCriterio { get; set; }
}
