using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class DynamicModule
{
    public int ModuleId { get; set; }

    public string Name { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string EntityName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedByUserId { get; set; }
}
