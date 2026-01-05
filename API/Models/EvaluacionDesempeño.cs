using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class EvaluacionDesempeño
{
    public int IdEvaluacionDesempeño { get; set; }

    public int? IdJefe { get; set; }

    public int? IdPersonalLegajo { get; set; }

    public DateTime? FechaEvaluacion { get; set; }

    public DateTime? FechaDesde { get; set; }

    public DateTime? FechaHasta { get; set; }

    public string? ResumenObjetivos { get; set; }

    public string? ResumenCompetencias { get; set; }

    public string? EvaluacionGeneral { get; set; }

    public string? Mantener { get; set; }

    public string? Adquirir { get; set; }

    public string? Abandonar { get; set; }

    public string? ObservacionEvaluado { get; set; }

    public int? Estado { get; set; }
}
