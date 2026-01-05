using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class PersonalLegajo
{
    public int IdPersonalLegajo { get; set; }

    public string? Apellido { get; set; }

    public string? Nombre { get; set; }

    public string? Calle { get; set; }

    public string? Nro { get; set; }

    public string? Piso { get; set; }

    public string? Depto { get; set; }

    public string? Telefono { get; set; }

    public string? Movil { get; set; }

    public int? IdDocumentoTipo { get; set; }

    public string? NroDocumento { get; set; }

    public string? Foto { get; set; }

    public int? IdNivelFormacion { get; set; }

    public int? IdEstadoCivil { get; set; }

    public DateTime? Nacimiento { get; set; }

    public int? IdNacionalidad { get; set; }

    public int? Sexo { get; set; }

    public string? Cuil { get; set; }

    public string? Categoria { get; set; }

    public DateTime? Ingreso { get; set; }

    public DateTime? Egreso { get; set; }

    public string? MesesAntiguedadReconocida { get; set; }

    public int? IdConvenio { get; set; }

    public int? IdArt { get; set; }

    public int? NroAfiliado { get; set; }

    public int? IdBanco { get; set; }

    public string? NroCajaAhorro { get; set; }

    public int? IdSeccion { get; set; }

    public int? IdSector { get; set; }

    public int? IdPuesto { get; set; }

    public int? IdJefe { get; set; }

    public bool? Jubilado { get; set; }

    public string? Tarjeta { get; set; }

    public string? MensajeIngreso { get; set; }

    public string? MensajeEgreso { get; set; }

    public string? FondoCompJub { get; set; }

    public string? AporteVoluntario { get; set; }

    public string? Observaciones { get; set; }

    public string? Archivo { get; set; }

    public int? Estado { get; set; }

    public int? Tipo { get; set; }

    public string? Email { get; set; }

    public int? IdProvincia { get; set; }

    public int? IdLocalidad { get; set; }

    public string? Edad { get; set; }

    public int? IdLegajo { get; set; }

    public int? LugarTrabajo { get; set; }

    public Guid? UserId { get; set; }

    public string? Firma { get; set; }

    public string? TokenNot { get; set; }
}
