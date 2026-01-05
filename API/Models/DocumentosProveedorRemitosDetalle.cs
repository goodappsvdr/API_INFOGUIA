using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosProveedorRemitosDetalle
{
    public int IdDocumentoProveeorRemitoDetalle { get; set; }

    public int? IdDocumentoProveedorRemito { get; set; }

    public int? IdItem { get; set; }

    public decimal? CantidadFactura { get; set; }

    public decimal? CantidadRemito { get; set; }

    public decimal? Saldo { get; set; }

    public int? IdDocumentoProveedor { get; set; }

    public int? IdRemito { get; set; }
}
