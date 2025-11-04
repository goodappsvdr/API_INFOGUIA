using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public int TenantId { get; set; }

    public string Name { get; set; } = null!;
}
