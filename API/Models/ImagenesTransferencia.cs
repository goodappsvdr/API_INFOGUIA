using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ImagenesTransferencia
{
    public int IdImagenTransferencia { get; set; }

    public int? IdTransferencia { get; set; }

    public int? IdRecibo { get; set; }

    public int? IdReciboDetalle { get; set; }

    public string? Imagen { get; set; }
}
