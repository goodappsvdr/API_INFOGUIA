using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class ItemsImpuesto
{
    public int IdItemImpuesto { get; set; }

    public int? IdItem { get; set; }

    public int? IdImpuesto { get; set; }
}
