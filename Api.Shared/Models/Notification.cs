using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Url { get; set; }

    public bool? Viwed { get; set; }

    public bool? Delete { get; set; }
}
