using System;
using System.Collections.Generic;

namespace Api.Shared.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public int TenantId { get; set; }

    public int? ParentCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? IconUrl { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ModifiedByUserId { get; set; }

    public virtual User? CreatedByUser { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual User? ModifiedByUser { get; set; }

    public virtual Category? ParentCategory { get; set; }

    public virtual Tenant Tenant { get; set; } = null!;
}
