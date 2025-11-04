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

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual City City { get; set; } = null!;

    public virtual User? CreatedByUser { get; set; }

    public virtual User? ModifiedByUser { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
