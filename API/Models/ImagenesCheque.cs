using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ImagenesCheque
{
    public int IdImagenCheque { get; set; }

    public int? IdEntidadCheque { get; set; }

    public int? IdRecibo { get; set; }

    public int? IdReciboDetalle { get; set; }

    public int? FrontalTrasera { get; set; }

    public string? Imagen { get; set; }
}
