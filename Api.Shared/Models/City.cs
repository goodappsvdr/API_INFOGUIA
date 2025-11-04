using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class City
{
    public int CityId { get; set; }

    public int ProvinceId { get; set; }

    public string Name { get; set; } = null!;

    public string? IconUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Province Province { get; set; } = null!;

    public virtual ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
}
