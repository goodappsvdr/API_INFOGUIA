using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string Name { get; set; } = null!;

    public string? IconUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();
}
