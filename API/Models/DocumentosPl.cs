using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class DocumentosPl
{
    public int IdDocumentoPl { get; set; }

    public int? IdDocumento { get; set; }

    public int? IdPersonalLegajo { get; set; }
}
