using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DatosEmpresa
{
    public int IdDatosEmpresa { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string NombreFantasia { get; set; } = null!;

    public string CuitEmpresa { get; set; } = null!;

    public int CondicionIva { get; set; }

    public DateTime FechaInicio { get; set; }

    public string 
        ccionComercial { get; set; } = null!;

    public int IdProvincia { get; set; }

    public int IdLocalidad { get; set; }

    public string Telefono { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string CuitPersona { get; set; } = null!;

    public string ClaveFiscal { get; set; } = null!;

    public string Logo { get; set; } = null!;
}
