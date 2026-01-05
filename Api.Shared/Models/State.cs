using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class State
{
    public int StateId { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public bool? State1 { get; set; }
}
