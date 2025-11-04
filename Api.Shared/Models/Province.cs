using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Province
{
    public int ProvinceId { get; set; }

    public int CountryId { get; set; }

    public string Name { get; set; } = null!;

    public string? IconUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Country Country { get; set; } = null!;
}
