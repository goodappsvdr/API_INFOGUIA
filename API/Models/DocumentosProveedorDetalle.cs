using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosProveedorDetalle
{
    public long IdDocumentoProveedorDetalle { get; set; }

    public long? IdDocumentoProveedor { get; set; }

    public int? IdItem { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Cantidad { get; set; }

    public string? ListaPrecio { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? Neto { get; set; }

    public decimal? Ivaalic { get; set; }

    public decimal? Iva { get; set; }

    public decimal? Otros { get; set; }

    public decimal? Total { get; set; }

    public int? IdImpuestoIva { get; set; }

    public int? IdListaPrecio { get; set; }

    public decimal? Metros { get; set; }

    public int? EstadoLibroIva { get; set; }

    public string? Observaciones { get; set; }

    public decimal? Bonificacion { get; set; }

    public decimal? NroTropa { get; set; }

    public string? ClasificacionFrigorifico { get; set; }

    public decimal? Kilos { get; set; }

    public decimal? NroCorrelativo { get; set; }

    public decimal? NroGarron { get; set; }

    public int? IdClasificacion { get; set; }

    public string? TipificacionProductiva { get; set; }

    public bool? Reservado { get; set; }

    public bool? Cuarteado { get; set; }

    public bool? Asignado { get; set; }

    public bool? Faenado { get; set; }

    public int? IdProveedor { get; set; }

    public int? IdDocumentoProveedorRomaneo { get; set; }

    public DateTime? FechaFaena { get; set; }
}
