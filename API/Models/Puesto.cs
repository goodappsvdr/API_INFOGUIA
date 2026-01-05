using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Puesto
{
    public int IdPuesto { get; set; }

    public string? Nombre { get; set; }

    public int? IdSector { get; set; }

    public string? Detalle { get; set; }

    public string? Experiencia { get; set; }

    public string? Funciones { get; set; }

    public bool? Activo { get; set; }

    public DateTime? Fecha { get; set; }

    public int? RespDecisionesImp { get; set; }

    public string? DetalleDecisionesImp { get; set; }

    public int? RespTrabOtPersonas { get; set; }

    public string? DetalleTrabOtPersonas { get; set; }

    public int? RespMateriales { get; set; }

    public string? DetalleMateriales { get; set; }

    public int? RespCustodia { get; set; }

    public string? DetalleCustodia { get; set; }

    public int? RespInfConfidencial { get; set; }

    public string? DetalleInfConfidencial { get; set; }

    public int? RespSegPersonas { get; set; }

    public string? DetalleSegPersonas { get; set; }

    public bool? Viajar { get; set; }

    public string? LugarResidencia { get; set; }

    public int? FormAcademicaReq { get; set; }

    public string? DescAcademicaReq { get; set; }

    public int? Estado { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}
