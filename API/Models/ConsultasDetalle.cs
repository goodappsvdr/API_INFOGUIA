using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ConsultasDetalle
{
    public int IdConsultaDetalle { get; set; }

    public int IdConsulta { get; set; }

    public int IdMotivo { get; set; }
}
