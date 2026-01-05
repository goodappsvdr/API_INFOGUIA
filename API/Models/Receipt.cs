using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class Receipt
{
    public int IdReceipt { get; set; }

    public string? Period { get; set; }

    public int? IdType { get; set; }

    public string? Liquidation { get; set; }

    public int? IdLegajo { get; set; }

    public string? Archive { get; set; }

    public DateTime? SendDate { get; set; }

    public DateTime? DisplayDate { get; set; }

    public DateTime? OpeningDate { get; set; }

    public DateTime? StateDate { get; set; }

    public int? IdState { get; set; }

    public bool? Signed { get; set; }
}
