using Api.Shared.DTOs.Tenant;
using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Help : BaseEntity
{
    public int Id { get; set; }

    public string? Question { get; set; }

    public string? Answer { get; set; }
}
