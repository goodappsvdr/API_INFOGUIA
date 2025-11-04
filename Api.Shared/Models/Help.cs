using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Help
{
    public int Id { get; set; }

    public string? Question { get; set; }

    public string? Answer { get; set; }
}
