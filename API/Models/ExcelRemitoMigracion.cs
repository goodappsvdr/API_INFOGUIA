using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ExcelRemitoMigracion
{
    public int IdExcelMigracion { get; set; }

    public string? Letra { get; set; }

    public int? Pto { get; set; }

    public int? Numero { get; set; }

    public string? Tropa { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Cuit { get; set; }

    public string? Cuenta { get; set; }

    public string? DenominacionCliente { get; set; }

    public string? Producto { get; set; }

    public string? Articulo { get; set; }

    public string? DenominacionArticulo { get; set; }

    public double? Medias { get; set; }

    public double? Kilos { get; set; }

    public double? Precio { get; set; }

    public string? Fact { get; set; }

    public string? Factura { get; set; }
}
