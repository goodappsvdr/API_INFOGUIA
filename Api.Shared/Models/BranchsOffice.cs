using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class BranchsOffice
{
    public int BranchOfficeId { get; set; }

    public string? Name { get; set; }

    public int? State { get; set; }

    public string? SalesPoint { get; set; }
}
