using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Tenant
{
    public int TenantId { get; set; }

    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedByUserId { get; set; }
}
