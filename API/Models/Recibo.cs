using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Recibo
{
    public int IdRecibo { get; set; }

    public string? Periodo { get; set; }

    public string? Convenio { get; set; }

    public int? Legajo { get; set; }

    public string? RazonSocial { get; set; }

    public string? Cuil { get; set; }

    public int? Seccion { get; set; }

    public decimal? RemuneracionBasica { get; set; }

    public decimal? Antiguedad { get; set; }

    public decimal? Presentismo { get; set; }

    public decimal? FuturoAumento { get; set; }

    public decimal? AdicionalFuncion { get; set; }

    public decimal? AdicionalFuncionV { get; set; }

    public decimal? AdicionalPartido { get; set; }

    public decimal? AsignacionComplementaria { get; set; }

    public decimal? Comisiones { get; set; }

    public decimal? DescDiasNoTrabajados { get; set; }

    public decimal? FallaCaja { get; set; }

    public decimal? FeriadoCompensatorio { get; set; }

    public decimal? FeriadoTrabajado { get; set; }

    public decimal? HorasExtras100 { get; set; }

    public decimal? HorasExtras50 { get; set; }

    public decimal? LicenciaEnfermedad { get; set; }

    public decimal? LicenciaSinGoceSueldo { get; set; }

    public decimal? LicenciasEspeciales { get; set; }

    public decimal? Sac1erSemestre { get; set; }

    public decimal? ViaticosPorViaje { get; set; }

    public decimal? ViaticosUtileros { get; set; }

    public decimal? TotalRem { get; set; }

    public decimal? RemuneracionBasicaNr { get; set; }

    public decimal? AntiguedadNr { get; set; }

    public decimal? PresentismoNr { get; set; }

    public decimal? FuturoAumentoNr { get; set; }

    public decimal? AdicionalFuncionNr { get; set; }

    public decimal? AdicionalFuncionVnr { get; set; }

    public decimal? AdicionalPartidoNr { get; set; }

    public decimal? AsignacionComplementariaNr { get; set; }

    public decimal? ComisionesNr { get; set; }

    public decimal? DescDiasNoTrabajadosNr { get; set; }

    public decimal? FallaCajaNoRem { get; set; }

    public decimal? FeriadoCompensatorioNr { get; set; }

    public decimal? FeriadoTrabajadoNr { get; set; }

    public decimal? HorasExtras100Nr { get; set; }

    public decimal? HorasExtras50Nr { get; set; }

    public decimal? LicenciaEnfermedadNr { get; set; }

    public decimal? LicenciasEspecialesNr { get; set; }

    public decimal? Premio { get; set; }

    public decimal? SacNoRem1erSemestre { get; set; }

    public decimal? ViaticosPorViajeNr { get; set; }

    public decimal? ViaticosUtilerosNr { get; set; }

    public decimal? TotalNoRem { get; set; }

    public decimal? Jubilacion { get; set; }

    public decimal? Ley19032 { get; set; }

    public decimal? ObraSocial { get; set; }

    public decimal? CuotaSindical { get; set; }

    public decimal? CuotaSolidaria { get; set; }

    public decimal? Rnss { get; set; }

    public decimal? ObraSocialJugador { get; set; }

    public decimal? ObraSocialTecnico { get; set; }

    public decimal? CuotaSindicalFutAgrem { get; set; }

    public decimal? SindicatoTecFutbol { get; set; }

    public decimal? AnticipoHaberes { get; set; }

    public decimal? Anticipo { get; set; }

    public decimal? ConvenioPrestamo { get; set; }

    public decimal? DescuentoAmutedyc { get; set; }

    public decimal? EmbargoCuotaAlimentaria { get; set; }

    public decimal? EmbargoJudicial { get; set; }

    public decimal? SueldoAnualComp { get; set; }

    public decimal? ImpuestoGananciaReintegro { get; set; }

    public decimal? ImpuestoGananciaRetencion { get; set; }

    public decimal? RetencionEspecial { get; set; }

    public decimal? TotalDescuento { get; set; }

    public decimal? TotalNeto { get; set; }
}
