using Api.Shared.DTOs.Tenant;
using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Politic : BaseEntity
{
    public string Type { get; set; } = null!;

    public string Value { get; set; } = null!;
}
