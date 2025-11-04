using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Parameter
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Value { get; set; }
}
